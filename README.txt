Alphabeta
---------------------
La fonction alphabeta est implémentée selon l'exemple du cours (alphabeta2)
et est appelée dans la fonction getNextMove, elle prend en paramètres:

-le plateau de jeu actuel
-la profondeur de recherche
-minOrMax (-1 si min et 1 si max)
-la valeur du parent (0 a l'appel)
-la couleur à jouer

Elle retourne un tuple avec:
-le score en double
-la coordonée x du coups à jouer en int
-la coordonée y du coups à jouer en int

Fonction d'évaluation
---------------------
librement inspirée de : https://kartikkukreja.wordpress.com/2013/03/30/heuristic-function-for-reversiothello/
la fonction d'évaluation se compose de 4 parties :

-différence de pièces, entourage des pièces et valeur des pièces:
    - Premièrement, on calcule le nombre de pièces pour chaque joueur (plus on a de pièces mieux c'est).
    - Deuxièmement, on calcule le poids de chaque pièce en fonction d'une matrice qui nous donne la pondération
    pour chaque case (par exemple,  une case dans un coin vaudra beaucoup de points alors qu'une case autour
    du coin, facilement retournable, se verra attribuer une pondération négative).
    - Troisièmement, on calcule la stabilité de chaque pièce : il s'agit de la probabilité qu'elle a
    d'être retournée au prochain coup en fonction du contenu des cases adjacentes : si ces dernières sont vides, la case est plus stable.
    
- Occupation des coins :
    les cases des coins ne pouvant pas être reprises, il s'agit des cases les plus importantes du jeu : 
    les possesseurs des coins seront donc recompensé par un score plus élevé. 

- Entourage des coins : 
    Si un coin un vide, les trois cases autour de ce dernier sont extrêmement importantes : un coup très fort peut potentiellement être joué par l'adversaire au tour suivant, ce dernier prenant le contrôle d'un coin.
    cette fonction renvoie donc un score négatif.
    (Une amélioration possible serait d'évaluer les coups possible d'un adversaire à partir de ces cases)

- Mobilité : 
    On calcule le nombre de coups possible pour nous et pour notre adversaire. Plus de coup possible -> plus d'espace pour jouer, et donc moins de chance d'arriver à une situation d'impasse..

Finalement, on pondère toutes ces valeurs en fonction de l'importance de chacune ce qui permet d'obtenir
un score optimal. Cette pondération n'est bien sûr pas faite au hasard : elle vient d'une étude de l'université de
Washington. (https://courses.cs.washington.edu/courses/cse573/04au/Project/mini1/RUSSIA/Final_Paper.pdf)
