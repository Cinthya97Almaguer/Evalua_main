#include <iostream>
#include <stdio.h>
#include <conio.h>
float resultado;
float area, radio, pi;
// Este programa calcula el volumen de un cilindro.
void main()
{
    printf("Radio = ");
    scanf("%d", &radio);
    pi=3.14159265358979323846;
    area = pi*(radio*radio);
    printf("\nArea = ");
    printf(area);  
}