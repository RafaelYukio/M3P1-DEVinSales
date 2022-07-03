# M3P1-DEVinSales
Projeto 1 - Módulos 3 do curso DEVInHouse (SENAI)  
Última Atualização: 03/07/2022

## Objetivos

Refatorar código feito a partir do M2P2, implementando autenticação por JWT, permissionamento por roles e testes.

<details>
  <summary>Requisitos</summary>
  
- Organização;
    - Realizar fork do repositório da M2P2;
    - Criar quadro no Trello para mapear tarefas;
- Autenticação por JWT, onde apenas usuário logados terão acesso a aplicação;
- Permissionamento por roles:
    - Administrador: Pode listar, criar, editar e deletar;
    - Gerente: Pode listar, criar e editar;
    - Usuário: Pode listar.
- Implementação de testes:
    - Cobertura de mais de 30% de testes utilizando NUnit.
</details>

<details>
  <summary>Critérios para máxima avaliação</summary>
  
- Criou um board no Trello e organizou com listas para modelo kanban e etiquetas de prioridade;
- Aluno criou o card, atribuiu seu nome nele e colocou as especificações de implementação no mesmo;
- Movimentou adequadamente o card, respeitando as regras no Kanbanize;
- A autenticação foi realizada e funciona perfeitamente;
- Todos os endpoints foram protegidos com as 3 regras de permissionamento;
- Acima de 30% da aplicação foi coberta de testes.
</details>

## Etapas

- Realizar fork;
- Migrar banco de dados do PostgreSQL para o SQL;
- Implementação do Identity;
- Implementação do JWT;
- Implementação dos testes.

## Usuários (logins)

<details>
  <summary>Usuários</summary>
  
- usuario1@dev.com.br
- usuario2@dev.com.br
- usuario3@dev.com.br
- usuario4@dev.com.br
    - senha: @Usuario1234

</details>

<details>
  <summary>Gerentes</summary>
  
- gerente1@dev.com.br
- gerente2@dev.com.br
    - senha: @Gerente1234

</details>

<details>
  <summary>Administrador</summary>
  
- admin@dev.com.br
    - senha: @Admin1234

</details>
</br>

Banco de dados populado automaticamente.

## Trello

https://trello.com/b/gWfk8wDH/m3p1-devinsales
