# 🧠 CSharpCachingLogging

Este projeto demonstra práticas de cache e logging em aplicações .NET, utilizando uma arquitetura modular para facilitar a manutenção e escalabilidade.

## 🛠️ Tecnologias Utilizadas

- .NET (versão especificada no projeto)
- C#
- ASP.NET Core
- Bibliotecas de logging e caching padrão do .NET

## 📁 Estrutura do Projeto

```

CSharpCachingLogging/
├── CSharp.Logging.API/                # API para gerenciamento de logs
├── CSharpCachingLogging.ApiService/   # Serviço principal da API
├── CSharpCachingLogging.AppHost/      # Host da aplicação
├── CSharpCachingLogging.Log.API/      # API específica para logs
├── CSharpCachingLogging.Log.Benchmark/# Benchmarking de logs
├── CSharpCachingLogging.ServiceDefaults/ # Configurações padrão de serviços
├── CSharpCachingLogging.Web/          # Interface web da aplicação
├── CSharpCachingLogging.sln           # Solução do Visual Studio
├── .gitignore
└── .gitattributes

````

## 🚀 Como Executar o Projeto

1. **Clone o repositório:**

   ```bash
   git clone https://github.com/thiagodsantana/CSharpCachingLogging.git
   cd CSharpCachingLogging
````

2. **Abra a solução no Visual Studio:**

   * Abra o arquivo `CSharpCachingLogging.sln`.

3. **Configure os projetos de inicialização:**

   * Defina `CSharpCachingLogging.AppHost` como projeto de inicialização.

4. **Execute a aplicação:**

   * Pressione `F5` ou clique em "Iniciar" no Visual Studio.

## 📄 Funcionalidades

* Implementação de caching para melhorar a performance da aplicação.
* Logging estruturado para facilitar o monitoramento e diagnóstico.
* Arquitetura modular para facilitar a manutenção e escalabilidade.

## 🧑‍💻 Autor

* **Thiago Darlei Santana**

  * [GitHub](https://github.com/thiagodsantana)
  * [Website Pessoal](http://www.thiagodarlei.com.br)

## 📄 Licença

Este projeto está licenciado sob a [MIT License](LICENSE).
