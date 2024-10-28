#include <SFML/Graphics.hpp>
#include <SFML/Window.hpp>
#include <iostream>
#include "include/jogo.h"

void iniciarMenu(sf::RenderWindow& window) {
    bool isRunning = true;

    while (isRunning) {
        sf::Event event;
        while (window.pollEvent(event)) {
            if (event.type == sf::Event::Closed)
                window.close();

            // Aqui, você pode adicionar lógica para detectar cliques nas opções
            if (event.type == sf::Event::KeyPressed) {
                if (event.key.code == sf::Keyboard::Escape) {
                    // Exibir janela de confirmação ao sair
                    if (confirmarSaida(window)) {
                        isRunning = false; // Encerrar o loop do menu
                    }
                }
            }
        }

        window.clear(sf::Color::Black);
        // Desenhar opções do menu aqui
        // Exemplo: Desenhar "Iniciar Jogo" e "Sair"
        window.display();
    }
}

bool confirmarSaida(sf::RenderWindow& window) {
    // Aqui você pode criar uma janela de confirmação
    std::cout << "Tem certeza que deseja sair? (s/n): ";
    char resposta;
    std::cin >> resposta;
    return (resposta == 's' || resposta == 'S');
}
