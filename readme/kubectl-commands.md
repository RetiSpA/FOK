List pods:
kubectl get pods

List deployments:
kubectl get deployments

List services:
kubectl get services

List endpoints:
kubectl get endpoints

List object with namespace:
kubectl get OBJECT -n [NAMESPACE]
kubectl get pods -n kube-system

Deploy:
kubectl create -f PATH_TO_YAML
kubectl create -f "C:\src\FOK\FokDeploymentGateway.yaml"

Delete object:
kubectl delete OBJECT OBJECT_NAME
kubectl delete deployment fok-deployment-gateway

Delete all objects:
kubectl delete OBJECT OBJECT_NAME --all

Connect to pod shell:
kubectl exec -it PODNAME -- /bin/bash
kubectl exec -it fok-deployment-gateway-d8b5cf76d-6ch8c -- /bin/bash

View pod logs:
kubectl logs PODNAME
kubectl logs kube-dns-86f4d74b45-7z669

Create secret to pull images from private registry
kubectl create secret docker-registry regcredcaretro --docker-server=https://index.docker.io/v1/  --docker-username=DOCKER_ID --docker-password=DOCKER_ID_PASSWORD --docker-email=DOCKER_ID_EMAIL

Create Ingress Controller
kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/master/deploy/mandatory.yaml
kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/master/deploy/provider/cloud-generic.yaml

Apply optional ConfigMap to Ingress
kubectl apply -f CONFIG_MAP_PATH

Create Web Dashboard:
create service: kubectl apply -f https://raw.githubusercontent.com/kubernetes/dashboard/v1.10.1/src/deploy/recommended/kubernetes-dashboard.yaml
get login token: ((kubectl -n kube-system describe secret default | Select-String "token:") -split " +")[1]
start proxy: kubectl proxy
go to: http://localhost:8001/api/v1/namespaces/kube-system/services/https:kubernetes-dashboard:/proxy/?namespace=kube-system
insert token in login form with token