# MCP Financeiro Backend – Servidor MCP para Consultoria Financeira

O MCP Financeiro Backend é um projeto backend desenvolvido em .NET, que implementa um servidor MCP (Model Context Protocol) voltado para demandas financeiras pessoais.
O objetivo do projeto é expor ferramentas financeiras que possam ser consumidas por LLMs compatíveis com MCP, permitindo análises, consultas e suporte financeiro automatizado, com integração a modelos locais via Ollama.

O projeto segue princípios de arquitetura limpa, separação de responsabilidades e organização orientada ao domínio, servindo tanto como solução funcional quanto como base de estudos para integração entre IA + backend financeiro.

## Funcionalidades Atuais
* Servidor MCP compatível com Model Context Protocol
* Exposição de ferramentas financeiras para consumo por LLMs
* Integração com modelos locais via Ollama
* Estrutura preparada para expansão de novas tools MCP
* Organização por camadas (Domain, Application, Web, MCPServer)
* Ambiente containerizado com Docker

## Arquitetura do projeto
```
McpFinanceiro.Application     // Camada de aplicação
  /Services                  // Casos de uso e regras de aplicação
  /Dtos

McpFinanceiro.Domain          // Núcleo da aplicação
  /Entities                  // Entidades de domínio
  /Interfaces                // Contratos (repositórios, serviços)
  /Exceptions

McpFinanceiro.MCPServer      // Servidor MCP
  /Tools                     // Ferramentas expostas via MCP
  /Handlers                  // Handlers de requisições MCP
  /Configurations

McpFinanceiro.Web            // Camada Web / API
  /Controllers
  /Extensions
  /Middlewares

Infrastructure               // Infraestrutura e persistência
  /Data
  /Repositories

docker-compose.yml           // Ambiente de produção
docker-compose.development.yml // Ambiente de desenvolvimento
.env-example                 // Exemplo de variáveis de ambiente
```

A estrutura segue conceitos de Clean Architecture, mantendo o domínio desacoplado de infraestrutura e frameworks.

## Tecnologias utilizadas
* .NET / ASP.NET Core
* C#
* Model Context Protocol (MCP)
* Ollama (LLMs locais)
* Docker & Docker Compose
* Clean Architecture
* Princípios de SOLID
* Integração com IA via ferramentas MCP

## Como executar o projeto
### 1. Clone o projeto
```bash
git clone https://github.com/vitormcampos/mcp-financeiro-backend.git
cd mcp-financeiro-backend
```

### 2. Configure as variáveis de ambiente
Crie um arquivo .env com base no exemplo:
```bash
cp .env-example .env
```
Ajuste as variáveis conforme necessário (URLs, portas, configurações do Ollama, etc.).

### 3. Execute com Docker (recomendado)
```bash
docker-compose up --build

# Ou para ambiente de desenvolvimento:

docker-compose -f docker-compose.development.yml up --build
```

## Objetivos do projeto
* Explorar a integração entre IA e sistemas financeiros
* Servir como referência para implementação de servidores MCP em .NET
* Facilitar a criação de assistentes financeiros baseados em LLMs
* Manter uma base de código organizada, extensível e desacoplada
