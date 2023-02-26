kubectl apply -f namespace.yaml
kubectl apply -f sqlserver.yaml
kubectl apply -f catalog.yaml
kubectl apply -f ordering.yaml
kubectl apply -f frontend.yaml

# see what's running
kubectl get pods -A
