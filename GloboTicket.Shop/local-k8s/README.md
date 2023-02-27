# Local K8s setup

## Setup Script(s)

1. run ```dashboard.ps1``` first to create the dashboard and system admin
2. Add secrets (see below for more details)
3. run ```apply-deployments.ps1``` to create the pods, dashboard can be used to few status

## Secrets

k8s secrets should be added via ```kubectl``` as follows:

    kubectl create secret generic (secret reference) --from-literal=(secretKey)=(secret value) -n (namespace)

Example:

    kubectl create secret catalog-db --from-literal=connectionString="" -n globoticket

reference this secret in the matching YAML file (in the above case catalog.YAML):

    - name: CONNECTIONSTRINGS__DEFAULT
      valueFrom:
        secretKeyRef: 
          name: catalog-db
          key: connectionString


