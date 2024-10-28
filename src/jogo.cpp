#include <SFML/Graphics.hpp>
#include <SFML/Window.hpp>
#include <iostream>
#include "include/jogo.h"

// Função para desenhar e exibir o menu principal
void mostrarMenu(sf::RenderWindow& window, sf::RectangleShape& botaoIniciarJogo, sf::RectangleShape& botaoSair, sf::Text& textoIniciarJogo, sf::Text& textoSair) {
    // Desenha os elementos do menu
    window.draw(botaoIniciarJogo);
    window.draw(textoIniciarJogo);
    window.draw(botaoSair);
    window.draw(textoSair);
}

// Função para iniciar o menu principal e lidar com os eventos
void iniciarMenu(sf::RenderWindow& window) {
    // Declaração dos botões e textos
    sf::Font font;
    if (!font.loadFromFile("assets/fonts/squealer/Squealer Embossed.otf")) {
        std::cerr << "Erro ao carregar fonte." << std::endl;
        return;
    }

    sf::Text textoIniciarJogo("Iniciar Jogo", font, 30);
    textoIniciarJogo.setFillColor(sf::Color::White);
    textoIniciarJogo.setPosition(200, 150);

    sf::RectangleShape botaoIniciarJogo(sf::Vector2f(200, 50));
    botaoIniciarJogo.setFillColor(sf::Color::Blue);
    botaoIniciarJogo.setPosition(200, 150);

    sf::Text textoSair("Sair", font, 30);
    textoSair.setFillColor(sf::Color::White);
    textoSair.setPosition(200, 250);

    sf::RectangleShape botaoSair(sf::Vector2f(200, 50));
    botaoSair.setFillColor(sf::Color::Blue);
    botaoSair.setPosition(200, 250);

    bool isRunning = true;

    while (isRunning) {
        sf::Event event;
        while (window.pollEvent(event)) {
            if (event.type == sf::Event::Closed) {
                window.close();
            }

            if (event.type == sf::Event::KeyPressed) {
                if (event.key.code == sf::Keyboard::Escape) {
                    if (confirmarSaida(window)) {
                        isRunning = false;
                    }
                }
            }

            if (event.type == sf::Event::MouseButtonPressed) {
                sf::Vector2i mousePos = sf::Mouse::getPosition(window);

                // Checa cliques nos botões
                if (botaoIniciarJogo.getGlobalBounds().contains(mousePos.x, mousePos.y)) {
                    std::cout << "Iniciar Jogo clicado" << std::endl;
                }
                if (botaoSair.getGlobalBounds().contains(mousePos.x, mousePos.y)) {
                    if (confirmarSaida(window)) {
                        isRunning = false;
                    }
                }
            }
        }

        window.clear(sf::Color::Black);

        // Chama a função para desenhar o menu, passando os botões e textos
        mostrarMenu(window, botaoIniciarJogo, botaoSair, textoIniciarJogo, textoSair);

        window.display();
    }
}

// Função de confirmação de saída
bool confirmarSaida(sf::RenderWindow& window) {
    std::cout << "Tem certeza que deseja sair? (s/n): ";
    char resposta;
    std::cin >> resposta;
    return (resposta == 's' || resposta == 'S');
}
