J'ai découpé le développement en plusieurs étapes : 
Etape 1 (6 heures)
Le menu de base, ainsi que les différents états du jeu pour switch d'un menu à l'autre
Des systèmes core (factory, pooling)
Les controles primaire du joueur (mouvement, tir automatique, et creation de data pour les armes)

Etape 2 (4 heures)
Gestion des collisions des projectiles
Système de vie et de dégats
Ajout des premiers effets de feedbacks pour l'agrément (FX et sons)
Tweaking des caméras 

Etape 3 (8 heures)
Amélioration des data d'armes, et gestion de la rareté et UI pour équiper
Ajout d'un système de vague d'ennemies en data
Finalisation de la boucle de gameplay

Etape 4 (8 heures)
Ajout de 2 comportements distincts d'ennemies
Ajout de système de drop (basique et buff pour l'expérience du prototype : 1 mort = 1 loot)
Fusion des armes (automatisé)
Mise en place de plusieurs types de projectiles

Etape 5 (5 heures)
Ajout des power up entre les vagues
Polish des menus avec des petites transitions de menus

les temps sont vraiment approximatifs, 
j'ai essayé de faire l'ajout de feedbacks de façon assez régulière dans le développement et ne pas faire un "rush" final donc j'ai peut être passé plus de temps sur certaines parties finalement

The features that were difficult for you and why:
La partie la plus dur, je dirais que c'est surtout la gestion des divers ratio de téléphone. 
C'est quelque chose que je n'ai pas fait depuis pas mal de temps et auquel j'avais oublié de me préparer. Donc que ce soit pour l'UI ou pour la gestion de la camera.
J'ai aussi essayé de faire des "pauses" de développement en ajoutant du décors et en faisant quelques FX, mais pour le coup ce genre d'habillage n'est pas une compétence que je maitrise

The features you think you could do better and how
Dans les perks, j'ai ajouté un tir ralentissant, et n'ayant qu'un effet de malus, je me suis dis qu'un système de gestion d'état (poison/feu/gel/etc) n'était pas nécessaire
mais ça ajoute un effet moyen en visuel et en logique
Dans le cas d'un prototype avec cette envergure, j'ai utilisé beaucoup de singleton, mais si le jeu avait à vocation d'être plus gros, je tenderais à les remplacer par exemple avec plus d'appel d'event pour limiter le couplage des scripts

What you would do if you could go a step further on this game
Alors déjà les 2 points dont je parle au dessus.
On pourrait penser aussi à un systeme de selection de niveau pour avoir plus de progression
Une randomisation des waves pour avoir moins le coté redondant
Ajouter de la sauvegarde
Ajouter des perks
Ajouter des armes
Ajouter des ennemies avec des behavior différents (tank, chargeur, fuyant, kamikaze, ou juste avec des projectiles différents)
Ajouter des boss aux runs pour rentabiliser l'usage d'une arme (comme l'epee longue autoAim dans mon proto) qui a vocation a taper des cibles uniques 
Ajouter plus d'équipement et les rendre améliorable pour ajoute des objectifs secondaires au joueur, autre qu'avancer dans les niveaux
Ajout de rewarded pour ajouter une extra life ou pour récupérer de l'argent ou des equipements

Any comment you may have
J'ai profité de ce test pour faire découvrir à des néophytes ce qu'était le développement de jeu vidéo.
J'avais un espèce de journal de bord sur le discord des gens avec qui je joue a World of Warcraft pour leur faire découvrir mon métier.
Ainsi aux différentes étapes, je leur montrais l'avancée avec une vidéo et un texte pour parler des ajouts 