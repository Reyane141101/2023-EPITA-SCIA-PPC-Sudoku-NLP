def affiche(txt):
    if (txt == None):
        print("Erreur de resolution")
        return
    #affichage de la grille en Python
    print("\n_____________")
    for i in range(9):
        print("|", end="")
        for j in range(9):
            print(txt[i][j], end="")
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

def verificationMat(solve):
    txt = [[0 for k in range(9)] for j in range(9)]
    if (solve == None):
        print("solve is none")
        return 2
    if (len(solve[0]) == 1):
        return verification(solve)
    for i in range(9):
        for j in range(9):
            if (len(solve[i][j]) != 1):
                return 1
            if (solve[i][j][0] == 0):
                return 1
            txt[i][j] = solve[i][j][0]

    return verification(txt)[1]








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

    solve = unicite(solve, reste, case)
    return solve, traitement



#appel récursif par catégorie
def traiteLigne(solve, traitement):
    s = 0
    for i in range (9):
        solve, traite = ligne(i, solve, traitement)
        s += traite
    if (s == 9):
        traitement = 1#VERIF
    return solve, traitement

def traiteColonne(solve, traitement):
    s = 0
    for i in range (9):
        solve, traite = colonne(i, solve, traitement)
        s += traite
    if (s == 9):
        traitement = 1#VERIF
    return solve, traitement

def traiteCarre(solve, traitement):
    s = 0
    for i in range (3):
        for j in range (3):
            solve, traite = carre(i*3, j*3, solve, traitement)
            s += traite
    if (s == 9):
        traitement = 1#VERIF
    return solve, traitement



def resolveur(solve, traitement):
    nb_bcle = 0
    traitement = verificationMat(solve)
    while (traitement == 1 and nb_bcle != 20):
        solve, traitement = traiteLigne(solve, traitement)
        if (traitement == 1):
            solve, traitement = traiteColonne(solve, traitement)
            traitement = verificationMat(solve)
        if (traitement == 1):
            solve, traitement = traiteCarre(solve, traitement)
            traitement = verificationMat(solve)
        nb_bcle += 1

    #verifie si tout est correcte sur ligne, colonne, carre a faire
    if (traitement == 2):
        #print("erreur quelquepart")
        return solve, traitement

    return solve, traitement


def isOK(lst):
    #compare lst à [1,2,3,4,5,6,7,8,9]
    #teste complet et cohérence
    lst.sort()
    if (int(lst[0]) == 0):
        #cas incomplet
        return False, 1
    for i in range(9):
        if (int(lst[i]) - 1 != i):
            #cas erreur
            return False , 2
    #cas correct
    return True, 0

def verification(txt):
    #verifi si absence d'incoherance
    #test carre et colonne
    for i in range(9):
        lstL = []
        lstC = []
        for j in range(9):
            lstL.append(txt[i][j])
            lstC.append(txt[j][i])
        if (isOK(lstC)[0] == False):
            #colonne[i][j] incorrecte
            return txt, 2
        if (isOK(lstL)[0] == False):
            return txt, 2
    #test carre
    equart = [0,1,2,9,10,11,18,19,20]
    pas= [0,3,3,21,3,3,21,3,3]
    for i in range (9):
        lst = []
        for j in range(9):
            equart[j] += pas[i]
            lst.append(txt[equart[j]//9][equart[j]%9])
        if (isOK(lst)[0] == False):
            return txt, 2
    return txt, 0

def copieSolve(solve):
    ret = [[[0 for k in range(9)] for j in range(9)] for i in range(9)]
    for i in range (9):
        for j in range(9):
            ret[i][j] = solve[i][j][:]
    return ret

def choix(solve):
    for i in range(9):
        for j in range(9):
            if (len(solve[i][j]) > 1):
                #si choix a faire
                for k in range(len(solve[i][j])):
                    copiesolve = copieSolve(solve)
                    copiesolve[i][j] = [copiesolve[i][j][k]]
                    tmpCopie, traitement = resolveur(copiesolve, 1)
                    traitement = verificationMat(tmpCopie)
                    if (traitement == 0):
                        return tmpCopie
    #absence de solution
    for i in range(9):
        for j in range(9):
            if (len(solve[i][j]) > 1):
                #si choix a faire
                for k in range(len(solve[i][j])):
                    copiesolve = copieSolve(solve)
                    copiesolve[i][j] = [copiesolve[i][j][k]]
                    tmpCopie, traitement = resolveur(copiesolve, 1)
                    tmpCopie2 = choix(copieSolve(tmpCopie))
                    traitement = verificationMat(tmpCopie2)
                    if (traitement == 0):
                        return tmpCopie2
    print("sortie de secours")
    return solve

def miseEnFormeTxt(txt):
    #creation matrice comme on la veux (matrice**3)
    tout=[1,2,3,4,5,6,7,8,9]
    #cas 0, possibilites, tout 
    #matrice solve : meilleurs manipulation matricielle
    #solve = tableau = [[[0 for k in range(9)] for j in range(9)] for i in range(9)]
    solve = [[[0 for k in range(9)] for j in range(9)] for i in range(9)]
    for i in range(9):
        for j in range(9):
            if (txt[i][j] == 0):
                solve[i][j] = tout.copy()
            else:
                solve[i][j] = [txt[i][j]]
    return solve

def miseEnForme(txt):
    #creation matrice comme on la veux (matrice**3)
    #transformation tuple -> matrice
    txt = list(txt)
    for i in range(len(txt)):
        txt[i] = list(txt[i])

    tout=[1,2,3,4,5,6,7,8,9]
    #cas 0, possibilites, tout 
    #matrice solve : meilleurs manipulation matricielle
    solve = [[[0 for k in range(9)] for j in range(9)] for i in range(9)]
    for i in range(9):
        for j in range(9):
            if (txt[i][j] == 0):
                solve[i][j] = tout.copy()
            else:
                solve[i][j] = [txt[i][j]]
    return solve




def main(txt):
    #transformation en matrice**3
    solve = miseEnForme(txt)

    #calcule
    traitement = 1
    #traitement = 0 #0:bon, 1:en cours, 2:error

    #solveurs = []
    solveurs = solve
    bcl = 0
    while (traitement == 1):
        bcl += 1
        solveurs, traitement = resolveur(solve, traitement)
        traitement = verificationMat(solveurs)
        if (traitement == 1):
            #pas fini, solveur bloquer, appel fonction initialisation aleatoire
            solveurs = choix(solveurs)
            traitement = verificationMat(solveurs)
    #solveurs = resolveur(solve)

    #remise en forme (matrice**3->matrice**2)
    matRet = [[0 for j in range(9)] for i in range(9)]
    for i in range(9):
        for j in range(9):
            if (len(solveurs[i][j]) == 1):
                matRet[i][j] = solveurs[i][j][0]
    matRet = verification(matRet)[0]
    return matRet


#txt = "902005403100063025508407060026309001057010290090670530240530600705200304080041950"
"""txt = "000000907000420180000705026100904000050000040000507009920108000034059000507000000"
affiche(txt)
rettt = (main(txt))
affiche(rettt)
"""
"""

instance = [[0,0,0,0,9,4,0,3,0],
            [0,0,0,5,1,0,0,0,7],
            [0,8,9,0,0,0,0,4,0],
            [0,0,0,0,0,0,2,0,8],
            [0,6,0,2,0,1,0,5,0],
            [1,0,2,0,0,0,0,0,0],
            [0,7,0,0,0,0,5,2,0],
            [9,0,0,0,6,5,0,0,0],
            [0,4,0,9,7,0,0,0,0]]
"""

#instance = ((0,0,0,0,9,4,0,3,0),
#            (0,0,0,5,1,0,0,0,7),
#            (0,8,9,0,0,0,0,4,0),
#            (0,0,0,0,0,0,2,0,8),
#            (0,6,0,2,0,1,0,5,0),
#            (1,0,2,0,0,0,0,0,0),
#            (0,7,0,0,0,0,5,2,0),
#            (9,0,0,0,6,5,0,0,0),
#            (0,4,0,9,7,0,0,0,0))
r = main(instance)
affiche(r)

"""explosion
#prendre en compte cas random si bloqué
"""

def StrToMat(txt):
    mat = [[0 for k in range(9)] for j in range(9)]
    for i in range(9):
        for j in range(9):
            mat[i][j] = int(txt[i*9+j])
    return mat
    #ret = [[[0 for k in range(9)] for j in range(9)] for i in range(9)]


def tempo_somme():
    cptr = 0
    cptr_echec = 0
    #txt=""
    for i in range(len(lst_grille)):
        txt2 = lst_grille[i]
        txt = StrToMat(txt2)
        resultat = main(txt)
        #print("resultat = = = = = = = = 469 =", resultat)
        bec = verification(resultat)[1]
        if (bec == 0):
            #if (resultat !=None):
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
        resultat = main(txt)
        print("resultat = = = = = = = = 469 =", resultat)
        if (resultat !=None):
            cptr += 1
            print("\n\n\n\n\n\n\n\n\n\n\n")
        else:
            cptr_echec += 1
            print("i=i=i=", i)
            return

    print ("cptr=", cptr, " & cptr_echec=", cptr_echec)
#tempo()