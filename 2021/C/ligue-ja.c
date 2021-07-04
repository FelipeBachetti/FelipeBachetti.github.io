#include<stdio.h>
#include<time.h>

int main(int argc, char* argv[])
{
    int matr[6][12];
    int l1, c1, l2, c2, aux, i, j, p=97, l=0, fimDoJogo=0;
    srand(time(NULL));

    //Adiciona as letras ao tabuleiro usando a tabela ASCII
    for(i=0; i<6; i++)
    {
        for(j=0; j<12; j++)
        {
            matr[i][j] = p;
            l++;
            if (l>3)
            {
                l=0;
                p++;
            }
        }
    }

    //Embaralha posições
    for(i=0; i<100; i++)
    {
        l1 = rand() % 6;
        c1 = rand() % 12;
        l2 = rand() % 6;
        c2 = rand() % 12;
        aux=matr[l2][c2];
        matr[l2][c2]=matr[l1][c1];
        matr[l1][c1]=aux;
    }
    while (fimDoJogo<36)
    {
        //Mostra o tabuleiro
        printf("   0  1  2  3  4  5  6  7  8  9 10 11\n");
        for(i=0; i<6; i++)
        {
            printf("%d ", i);
            for(j=0; j<12; j++)
            {
                printf("[%c]", matr[i][j]);
            }
            printf("\n");
        }
        printf("Escreva as coordenadas (linha1 coluna1 linha2 coluna2): ");
        scanf("%d %d %d %d", &l1, &c1, &l2, &c2);
        //Checa se o posicionamento é válido
        if (l1>5 || l1<0 || l2>5 || l2<0 || c1>11 || c1<0 || c2>11 || c2<0)
        {
            printf("Coordenada(s) fora da matriz\n");
            continue;
        }
        if ((l1!=l2 && c1!=c2) || (l1==l2 && c1==c2))
        {
            printf("Coordenadas nao estao na mesma horizontal ou vertical, ou estao na mesma posicao\n");
            continue;
        }
        if ((matr[l1][c1]== NULL) || (matr[l2][c2]== NULL))
        {
            printf("Pelo menos uma das coordenadas nao possui letra\n");
            continue;
        }
        if (matr[l1][c1]==matr[l2][c2])
        {
            fimDoJogo++;
            //Apaga as letras escolhidas
            matr[l1][c1] = NULL;
            matr[l2][c2] = NULL;
            //Desce as letras que estão acima
            while (l1>0)
            {
                matr[l1][c1] = matr[l1-1][c1];
                matr[l1-1][c1] = NULL;
                l1--;
            }
            while (l2>0)
            {
                matr[l2][c2] = matr[l2-1][c2];
                matr[l2-1][c2] = NULL;
                l2--;
            }
        }
        else
        {
            printf("Errou\n");
        }
    }
    printf("Voce venceu");
    return 0;
}
