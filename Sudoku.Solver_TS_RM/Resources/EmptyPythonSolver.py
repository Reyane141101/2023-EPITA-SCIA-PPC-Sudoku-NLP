instance = ((0,0,0,0,9,4,0,3,0),
            (0,0,0,5,1,0,0,0,7),
            (0,8,9,0,0,0,0,4,0),
            (0,0,0,0,0,0,2,0,8),
            (0,6,0,2,0,1,0,5,0),
            (1,0,2,0,0,0,0,0,0),
            (0,7,0,0,0,0,5,2,0),
            (9,0,0,0,6,5,0,0,0),
            (0,4,0,9,7,0,0,0,0))


def affiche(txt):
    if (txt == None):
        print("Erreur de resolution")
        return
    #affichage de la grille en Python
    print("\n_____________")
    for i in range(9):
        print("|", end="")
        for j in range(9):
            print(txt[i*9+j], end="")
            if ((j+1) % 3 == 0):
                print("|", end="")
        if ((i+1) % 3 == 0):
            print("\n_____________", end="")
        print('')

def unicite(solve, reste, case):
    #cherche si unicite des possibilite pour les elts de reste
    for elt in range(len(reste)):
        cptr, pos=0, []
        for [i, j] in case:
            if reste[elt] in solve[i][j]:
                pos = [i,j]
                cptr +=1
        if (cptr == 1):
            #si unicite de le solution, on ajoute l'elt
            solve[pos[0]][pos[1]] = [reste[elt]]
    return solve

#appeler récursivement par catégorie
def ligne(i, solve, traitement):
    #nettoie les possibilités inutiles par lignes
    traiter = []
    indice = [] #traitement
    case = []
    for j in range(9):
        #recherche des chiffres deja traites
        if (len(solve[i][j]) == 1):
            traiter.append(int(solve[i][j][0]))
        else:
            indice.append(j)
    if (len(traiter) == 9):
        # ligne complete
        return solve, 1

    #reste chiffre a placer
    reste = []
    for j in range(9):
        if (j+1) not in traiter:
            reste.append(j+1)

    for j in indice:
        #supprime élt si présent sur la ligne
        for elt in range (1,10):
            if (elt in solve[i][j]):
                if elt in traiter:
                    solve[i][j].remove(elt)

    solve = unicite(solve, reste, case)
    return solve, traitement


def colonne(i, solve, traitement):
    #nettoie les possibilités inutiles par colonnes
    #ATTENTION, i : colonne, j : ligne
    traiter = []
    indice = [] #traitement
    case = []
    for j in range(9):
        #recherche des chiffres deja traites
        if (len(solve[j][i]) == 1):
            traiter.append(int(solve[j][i][0]))
        else:
            indice.append(j)
            case.append([j,i])
    if (len(traiter) == 9):
        # ligne complete
        return solve, 1

    #reste chiffre a placer
    reste = []
    for j in range(9):
        if (j+1) not in traiter:
            reste.append(j+1)

    for j in indice:
        #supprime élt si présent sur la colonne
        for elt in range (1,10):
            if (elt in solve[j][i]):
                if elt in traiter:
                    solve[j][i].remove(elt)

    solve = unicite(solve, reste, case)
    return solve, traitement

def carre(iParam, jParam, solve, traitement):
    #nettoie les possibilités inutiles par carre
    traiter = []
    indice = [] #traitement
    case = []
    for i in range(3):
        for j in range(3):
            #recherche des chiffres deja traites
            if (len(solve[i+iParam][j+jParam]) == 1):
                traiter.append(int(solve[i+iParam][j+jParam][0]))
            else:
                indice.append([i,j])
                case.append([i+iParam,j+jParam])
    if (len(traiter) == 9):
        # ligne complete
        return solve, 1

    #reste chiffre a placer
    reste = []
    for j in range(9):
        if (j+1) not in traiter:
            reste.append(j+1)

    for [i,j] in indice:
        #supprime élt si présent sur la ligne
        for elt in range (1,10):
            if (elt in solve[i+iParam][j+jParam]):
                if elt in traiter:
                    solve[i+iParam][j+jParam].remove(elt)

    """print("126__________126________126")
    print("reste=", reste, "case=", case)"""
    #print(solve)
    solve = unicite(solve, reste, case)
    #print(solve)
    return solve, traitement



#appel récursif par catégorie
def traiteLigne(solve, traitement):
    s = 0
    for i in range (9):
        solve, traite = ligne(i, solve, traitement)
        s += traite
    if (s == 9):
        traitement = 1
    return solve, traitement

def traiteColonne(solve, traitement):
    s = 0
    for i in range (9):
        solve, traite = colonne(i, solve, traitement)
        s += traite
    if (s == 9):
        traitement = 1
    return solve, traitement

def traiteCarre(solve, traitement):
    s = 0
    for i in range (3):
        for j in range (3):
            solve, traite = carre(i*3, j*3, solve, traitement)
            s += traite
    if (s == 9):
        traitement = 1
    #print("161____________161\n\n\n")
    return solve, traitement



def resolveur(txt):
    tout=[1,2,3,4,5,6,7,8,9]
    #cas 0, possibilites, tout 
    #matrice solve : meilleurs manipulation matricielle
    solve = tableau = [[[0 for k in range(9)] for j in range(9)] for i in range(9)]
    print(txt)
    for i in range(9):
        for j in range(9):
            #print("ici=",txt[i*9+j])
            #if (txt[i*9+j] == '0'):
            if (txt[i][j] == 0):
                solve[i][j] = tout.copy()
            else:
                solve[i][j] = [txt[i][j]]
    traitement = 0 #0:bon, 1:fini, 2:error
    nb_bcle = 0
    while (traitement == 0 and nb_bcle != 20):
        solve, traitement = traiteLigne(solve, traitement)
        if (traitement == 0):
            solve, traitement = traiteColonne(solve, traitement)
        if (traitement == 0):
            solve, traitement = traiteCarre(solve, traitement)
        nb_bcle += 1

    #verifie si tout est correcte sur ligne, colonne, carre a faire
    if (traitement == 2):
        print("erreur quelquepart")
        return txt

    """
    #met à jour valeur de txt
    for i in range(9):
        for j in range(9):
            print("ici et la", len(solve[i][j]))
            if (len(solve[i][j]) == 1):
                txt[i*9+j] = solve[i][j][0]
                txt[i*9+j] = solve[i][j][1]
    """
    #affiche(solve)
    return solve


def isOK(lst):
    #compare lst à [1,2,3,4,5,6,7,8,9]
    #teste complet et cohérence
    lst.sort()
    for i in range(9):
        if (int(lst[i]) - 1 != i):
            return False
    return True

def verification(txt):
    #verifi si absence d'incoherance
    #test carre et colonne
    for i in range(9):
        lstL = []
        lstC = []
        for j in range(9):
            # sauvegarde ligne
            lstL.append(txt[i][j])
            # sauvegarder cols
            lstC.append(txt[j][i])
        if (isOK(lstC) == False):
            print("laC=c=c=", i, j)
            return None
        if (isOK(lstL) == False):
            print("laL=l=l=", i, j)
            return None
    #test carre
    equart = [0,1,2,9,10,11,18,19,20]
    pas= [0,3,3,21,3,3,21,3,3]
    for i in range (9):
        lst = []
        for j in range(9):
            equart[j] += pas[i]
            lst.append(txt[equart[j]])
        if (isOK(lst) == False):
            print(equart)
            return None
    return txt

def main(txt):
    # convertir le tuple en liste
    txt = list(txt)
    for i in range(len(txt)):
        txt[i] = list(txt[i])
    #calcule
    solveurs = resolveur(txt)
    #remise en forme
    for i in range(9):
        for j in range(9):
            solveurs[i][j] = solveurs[i][j][0]
    txt = verification(txt)
    if (None in solveurs):
        return [0]
    return solveurs

r = main(instance)

def tempo_somme():
    cptr = 0
    cptr_echec = 0
    txt=""
    for i in range(len(lst_grille)):
        txt = lst_grille[i]
        if (main(txt) !=None):
            cptr += 1
        else:
            cptr_echec += 1

    print ("cptr=", cptr, " & cptr_echec=", cptr_echec)
#tempo_somme()

def tempo():
    cptr = 0
    cptr_echec = 0
    txt=""
    for i in range(len(lst_grille)):
        txt = lst_grille[i]
        if (main(txt) !=None):
            cptr += 1
            print("\n\n\n\n\n\n\n\n\n\n\n")
        else:
            cptr_echec += 1
            print("i=i=i=", i)
            return

    print ("cptr=", cptr, " & cptr_echec=", cptr_echec)
