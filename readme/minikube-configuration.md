### Configurazione Minikube ###

### Disabilitazione Hyper-V (la macchina effettuerà un riavvio automatico per completare la procedura)

Aprire la PowerShell (amministratore) e digitare il seguente comando:
Disable-WindowsOptionalFeature -Online -FeatureName Microsoft-Hyper-V-All

### Installazione VirtualBox
Lanciare l'eseguibile VirtualBox-6.0.4-128413-Win e seguire le finestre di dialogo fino alla completa installazione

### Installazione Docker
Lanciare l'eseguibile DockerToolbox
Dalla lista dei componenti da installare deselezionare tutto e lasciare solo
* Docker Client for Windows
* Docker Machine for Windows
* Docker Compose for Windows
* Git for Windows

NOTA BENE: NON selezionare VirtualBox, perché è già presente!!

Lanciare il programma Docker Quickstart Terminal e attendere che sia completamente partito.
Si aprirà un prompt CMD, in caso di stuck premere invio per rinfrescare la sessione.

### Installazione Minikube + Kubectl
Lanciare l'eseguibile minikube-installer
(potrebbe essere segnalato come virus, ignorare l'avvertimento e seguire l'installazione).

Finita l'installazione spostare l'eseguibile kubectl.exe all'interno della directory di
installazione di minikube (es: C:\Program Files (x86)\Kubernetes\Minikube)

Aprire la PowerShell e digitare il seguente comando:
minikube.exe start --vm-driver=virtualbox

Attendere il messaggio di corretto avvio del cluster.

### PREPARAZIONE ###
Il primo avvio di ogni componente esegue degli step di post-installazione.
Le operazioni richiederanno una connesione internet per poter scaricare le ISO dalla rete e richiederanno
diversi minuti ciascuna.
	