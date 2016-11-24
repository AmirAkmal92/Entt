 
 param(
       
     $NodeName = "S322@S322",
     $Master = "S323@S323"
 
 )
 

 $env:RABBITMQ_BASE="C:\rabbitmq_base"
 $env:ERLANG_HOME="C:\Program Files\erl8.0"

 #TODO : set the the cookies in the Node to the one from master


 rabbitmqctl.bat -n $NodeName stop_app
 rabbitmqctl.bat -n $NodeName join_cluster $Master
 rabbitmqctl.bat -n $NodeName start_app

 rabbitmqctl.bat -n $NodeName add_vhost PosEntt

.\rabbitmqctl.bat -n $NodeName add_user PosEnttApp megadeth
.\rabbitmqctl -n $NodeName set_user_tags PosEnttApp administrator


.\rabbitmqctl -n $NodeName set_permissions -p PosEntt PosEnttApp ".*" ".*" ".*"

.\rabbitmq-plugins.bat -n $NodeName enable rabbitmq_management