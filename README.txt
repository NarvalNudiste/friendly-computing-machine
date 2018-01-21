Alphabeta
---------------------
La fonction alphabeta est implémentée selon l'exemple du cours (alphabeta2)
et est appelée dans la fonction getNextMove, elle prend en paramètres:
-le board actuel
-le niveau de recherche
-minOrMax (-1 si min et 1 si max)
-la valeur du parent (0 a l'appels)
-la couleur à jouer

et elle retourne un tuple avec:
-le score en double
-la coordonée x du coups à jouer en int
-la coordonée y du coups à jouer en int

Fonction d'évaluation
---------------------
librement inspirée de : https://kartikkukreja.wordpress.com/2013/03/30/heuristic-function-for-reversiothello/
la fonction d'évaluation se compose de 4 parties :

-différence de pièces, entourage des pièces et valeur des pièces:
    -premièrement on calcul le nombre de pièces pour chaque joueur (plus on a de pièces mieu c'est).
    -deuxièmement on calcul le poids de chaque pièce en fonction d'une matrice qui nous donne la pondération
    pour chaque case par exemple une case dans un coin vaudra beaucoup de points alors qu'une case autour
    du coin qui est facilement retournable vaudra des points négatifs.
    -troisièmement on calcul la stabilité de chaque pièce c'est à dire la chance qu'elle a
    d'être retournée au prochain coups en regardant si les cases entourant la case testée sont vide
    biensur plus notre jeu est stable mieu c'est.
    
-occupation des coins
    on calcul qui occcupe les coins car les coins sont les cases les plus importantes du jeu,
    en effet un coin pris ne peu pas être repris.

-entourage des coins
    on calcul qui se trouve dans les 3 cases autour des coins si les coins sont vide,
    en effet jouer dans ces case donne la possibilité à l'adversaire de jouer dans les coins
    don d'avoir un coups très fort ce qu'on ne veut pas.

-mobilité
    on calcul le nombre de coups possible pour nous et notre adversaire le but ici étant d'avoir le plus
    de coups possibles ce qui donne plus d'espace pour jouer donc un avantage sur l'adversaire car moins
    de chance de devoir passer.
Finalement on pondère toutes ces valeurs en fonction de l'importance de chacune ce qui permet d'obtenir
un score optimal. Cette pondération n'est biensur pas faite au hasard ell vient d'une étude de l'université de
Washington. (https://courses.cs.washington.edu/courses/cse573/04au/Project/mini1/RUSSIA/Final_Paper.pdf)
