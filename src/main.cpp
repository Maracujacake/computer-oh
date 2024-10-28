#include <SFML/Graphics.hpp> 
#include "include/jogo.h" // funções específicas para o jogo

int main() {
    sf::RenderWindow window(sf::VideoMode(800, 600), "Jogo de Cartas");

    // Função de inicialização do jogo/menu
    iniciarMenu(window);

    return 0;
}
