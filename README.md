# ML-Agents Project IA 2024

## Installation

Suivez les étapes ci-dessous pour installer et configurer le projet.

### Prérequis

- [Unity](https://unity.com/) installé sur votre machine.
- [Python 3.9.13](https://www.python.org/downloads/release/python-3913/) installé sur votre machine.

### Étapes d'installation

1. Clonez le dépôt du projet sur votre machine locale :

   ```sh
   git clone https://github.com/11gar/ML-Agents-Project-IA-2024
   cd ML-Agents-Project-IA-2024
   ```

2. Installez les dépendances Python :

   ```sh
   pip install -r config.txt
   ```

3. Ouvrez le projet Unity :

   - Lancez Unity Hub.
   - Cliquez sur "Add" et sélectionnez le dossier du projet cloné.

4. Si ce n'est pas déjà fait, configurez ML-Agents dans Unity :
   - Window
   - Package Manager
   - En haut à droite, chercher ml-agents
   - Ajouter au projet

### Exécution

1. Assurez-vous que toutes les dépendances sont installées.
2. Ouvrez le projet dans Unity.
3. Ouvrez un terminal à la racine du projet
4. Activez l'environnement virtuel :

   ```sh
   venv\Scripts\activate
   ```

5. mlagents-learn trainer_config.yaml --run-id=16 --resume
6. Lancez l'exécution via l'interface Unity.

### Ressources supplémentaires

- [Documentation Unity](https://docs.unity3d.com/Manual/index.html)
- [Documentation ML-Agents](https://github.com/Unity-Technologies/ml-agents/blob/main/docs/Readme.md)
