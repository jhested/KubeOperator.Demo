# Kubernetes Operator Demo

## Sample CRD

```
apiVersion: activedirectory.operatordemo.dk/v1alpha1
kind: WindowsServiceAccount
metadata:
  name: activedirectoryoperator
  namespace: default
spec:
  name: activedirectoryoperator
  memberships:
    - groupName: directory_administrators
```

## Links

- KubeOps C# Operator Framework: https://github.com/buehler/dotnet-operator-sdk
- Operator Pattern: https://kubernetes.io/docs/concepts/extend-kubernetes/operator/
- CNCF Operator White Paper: https://github.com/cncf/tag-app-delivery/blob/163962c4b1cd70d085107fc579e3e04c2e14d59c/operator-wg/whitepaper/Operator-WhitePaper_v1-0.md
- KStatus: https://github.com/kubernetes-sigs/cli-utils/blob/master/pkg/kstatus/README.md
