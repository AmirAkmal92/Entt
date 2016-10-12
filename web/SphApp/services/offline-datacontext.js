define([], function () {
    var arrayTypeNamePattern = /\[/,
        toObservable = function (item) {
            if (typeof item === "function") return item;
            if (typeof item === "number") return item;
            if (typeof item === "string") return item;
            if (typeof item.$type === "undefined") return item;
            if (_(item.$type).isNull()) return item;

            var pattern = /Bespoke\..*?\..*?\.(.*?),/,
                $typeFieldValue = item.$type,
                type = "",
                partial = null;

            if (typeof item.$type === "function") {
                $typeFieldValue = item.$type();
            }
            if (typeof $typeFieldValue === "string") {
                var bespokeTypeMatch = pattern.exec($typeFieldValue);
                if (bespokeTypeMatch) {
                    type = bespokeTypeMatch[1];
                } else {
                    return item;
                }
            }

            for (var name in item) {
                (function (prop) {

                    var _propertyValue = _(item[prop]);

                    if (_propertyValue.isArray()) {

                        var children = _propertyValue.map(function (x) {
                            return toObservable(x, pattern);
                        });

                        item[prop] = ko.observableArray(children);
                        return;
                    }

                    if (_propertyValue.isNumber()
                        || _propertyValue.isNull()
                        || _propertyValue.isNaN()
                        || _propertyValue.isDate()
                        || _propertyValue.isBoolean()
                        || _propertyValue.isString()) {
                        item[prop] = ko.observable(item[prop]);
                        return;
                    }

                    if (_propertyValue.isObject()) {
                        var $typeFieldValue2 = item[prop].$type;

                        if ($typeFieldValue2 && arrayTypeNamePattern.exec($typeFieldValue2)) {
                            if (_(item[prop].$values).isArray()) {
                                var childItems = _(item[prop].$values).map(function (v) {
                                    return toObservable(v, pattern);
                                });
                                item[prop] = ko.observableArray(childItems);
                            }
                            return;
                        }

                        if (typeof item[prop] === "function") {
                            return;
                        }

                        var child = toObservable(item[prop], pattern);
                        item[prop] = ko.observable(child);
                        return;
                    }

                })(name);

            }

            if (bespoke.sph.domain[type + "Partial"]) {
                partial = new bespoke.sph.domain[type + "Partial"](item);
            }
            if (partial) {
                // NOTE :copy all the partial, DO NO use _extend as it will override the original value
                // if there is item with the same key
                for (var prop1 in partial) {
                    if (!item[prop1]) {
                        item[prop1] = partial[prop1];
                    }
                }
            }

            // if there are new fields added, chances are it will not be present in the json,
            // even it is, it would be nice to add WebId for those that are still missing one

            if (bespoke.sph.domain[type]) {
                var ent = new bespoke.sph.domain[type](system.guid());
                for (var prop2 in ent) {
                    if (!item[prop2]) {
                        item[prop2] = ent[prop2];
                    }
                }
            }
            // addChildItemFunction
            item.addChildItem = function (list, childType) {
                return function () {
                    if (typeof childType === "function") {
                        list.push(new childType(system.guid()));
                        return;
                    }
                    console.log("Whoaaaaa");
                };
            };

            item.removeChildItem = function (list, obj) {
                return function () {
                    list.remove(obj);
                };
            };


            return item;

        },
        name = ko.observable(),
        db = null,
        _options = null,
        openAsync = function (options) {
            _options = options;
            var version = 2;
            var promise = new Promise(function (resolve, reject) {


                var request = indexedDB.open(options.store.toLowerCase() + '_db', version);

                request.onupgradeneeded = function (e) {
                    db = e.target.result;
                    e.target.transaction.onerror = indexedDB.onerror;

                    if (db.objectStoreNames.contains(options.store)) {
                        db.deleteObjectStore(options.store);
                    }
                    var store = db.createObjectStore(options.store, {autoIncrement: true});
                };

                request.onsuccess = function (e) {
                    db = e.target.result;
                    resolve();
                };

                request.onerror = request.onblocked = function (e) {
                    console.log(e);
                    reject("Couldn't open DB");
                };
            });
            return promise;
        },
        loadAsync = function () {
            var todosArr = [];

            if (!db) {
                return;
            }
            var trans = db.transaction([_options.store], "readwrite");
            var store = trans.objectStore(_options.store);


            var promise = new Promise(function (resolve, reject) {

                var keyRange = IDBKeyRange.lowerBound(0);
                var cursorRequest = store.openCursor(keyRange);


                cursorRequest.onsuccess = function (e) {
                    var result = e.target.result;
                    if (result === null || result === undefined) {
                        resolve(todosArr);
                    }
                    else {
                        todosArr.push(toObservable(result.value));
                        result.continue();
                    }
                };

                cursorRequest.onerror = function (e) {
                    reject("Couldn't fetch items from the DB");
                };
            });
            return promise;
        },
        deleteAsync = function(webId){
            var promise = new Promise(function (resolve, reject) {
                var trans = db.transaction([_options.store], 'readwrite');
                var store = trans.objectStore(_options.store);

                var request = store.delete(webId);

                request.onsuccess = function (e) {
                    resolve();
                };

                request.onerror = function (e) {
                    console.log(e.value);
                    reject("Couldn't delete the passed item");
                };
            });
            return promise;
        },
        saveAsync = function (item) {

            var promise = new Promise(function (resolve, reject) {
                var trans = db.transaction([_options.store], 'readwrite');
                var store = trans.objectStore(_options.store);
                console.log("Saving..", ko.unwrap(item));

                var clone = JSON.parse(ko.mapping.toJSON(item));
                var request = store.put(clone);

                request.onsuccess = function (e) {
                    console.log(e);
                    resolve();
                };

                request.onerror = function (e) {
                    console.log(e);
                    reject("Couldn't add the passed item");
                };
            });
            return promise;
        },
        removeDbAsync = function () {
            if (typeof db !== 'undefined') {
                db.close();
                console.log('Attempting to delete the database...');

                var request = indexedDB.deleteDatabase('InitDB');

                request.onerror = request.onblocked = console.log;

                request.onsuccess = function () {
                    console.log('Database deleted');
                };

            } else {
                console.log('You must first create a database before attempting a delete.');
            }
        },
        loadOneAsync = function(webId){
            var c = openAsync(_options)
                .then(function(){
                    return loadOneAsync1(webId);
                });
            return c;
        },
        loadOneAsync1 = function(webId){
            var promise = new Promise(function (resolve, reject) {
                var trans1 = db.transaction([_options.store], 'readonly');
                var store1 = trans1.objectStore(_options.store);

                var request = store1.get(webId);

                request.onsuccess = function (e) {
                    console.log(e);
                    var item = toObservable(e.target.result);
                    resolve(item);
                };

                request.onerror = function (e) {
                    console.log(e.value);
                    reject("Couldn't add the passed item");
                };
            });
            return promise;
        },
        sendAsync = function(options){
            var method = options.method || "GET",
                contentType = options.contentType || "application/json",
                url = options.url,
                body = options.body || options.json;

            return new Promise(function(resolve, reject) {
                // Do the usual XHR stuff
                var req = new XMLHttpRequest();
                req.open(method, url);
                req.setRequestHeader("Content-type", contentType);

                req.onload = function() {
                    if (req.status == 200) {
                        resolve(JSON.parse(req.response));
                    }
                    else {
                        reject(Error(req.statusText));
                    }
                };
                req.onerror = function() {
                    reject(Error("Network Error"));
                };
                if(method === "POST"|| method === "PUT"){
                    req.send(body);
                }else{
                    req.send();
                }
            });
        };


    var vm = {
        removeDbAsync : removeDbAsync,
        name: name,
        openAsync: openAsync,
        deleteAsync: deleteAsync,
        loadOneAsync: loadOneAsync,
        loadAsync: loadAsync,
        saveAsync: saveAsync,
        sendAsync : sendAsync
    };


    return vm;

});
