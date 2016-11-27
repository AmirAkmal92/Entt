 
 param(
       
     $NodeName = "S322@S322",
     $Master = "S323@S323"
 
 )
 

 $env:RABBITMQ_BASE="C:\rabbitmq_base"
 $env:ERLANG_HOME="C:\Program Files\erl8.0"

 #TODO : set the the cookies in the Node to the one from master


.\rabbitmqctl.bat -n $NodeName stop_app
.\rabbitmqctl.bat -n $NodeName join_cluster $Master
.\rabbitmqctl.bat -n $NodeName start_app

.\rabbitmqctl.bat -n $NodeName add_vhost PosEntt

.\rabbitmqctl.bat -n $NodeName add_user PosEnttApp megadeth
.\rabbitmqctl -n $NodeName set_user_tags PosEnttApp administrator


.\rabbitmqctl -n $NodeName set_permissions -p PosEntt PosEnttApp ".*" ".*" ".*"

.\rabbitmq-plugins.bat -n $NodeName enable rabbitmq_management


.\rabbitmqctl.bat -n $NodeName cluster_status

.\rabbitmq-plugins.bat list

<#


 Note for the guys infra unit , whomever got to take care of this

 The RabbitMq is set on "ignore" mode for network partition, see CAP theorm, so it doens't handle network partition well 
 for increare in consistency and availability. assuming than Node 1 and Node 2 is in a very reliable network,
 connecting through the same network switch


 Things to be aware
While you could suspend a cluster node by running it on a laptop and closing the lid, 
the most common reason for this to happen is for a virtual machine to have been suspended by the hypervisor. 
While it's fine to run RabbitMQ clusters in virtualised environments, 
you should make sure that VMs are not suspended while running.
 Note that some virtualisation features such as migration of a VM from one host to another will tend to involve the VM being 
suspended.

Partitions caused by suspend and resume will tend to be asymmetrical - 
the suspended node will not necessarily see the other nodes as having gone down, but will be seen as down by the rest 
of the cluster. This has particular implications for pause_minority mode.

#>