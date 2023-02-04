# Desafio Técnico Inoa - Broker

**Este programa foi desenvolvido como parte do processo seletivo da empresa [Inoa](https://www.inoa.com.br/).**

## Executando o projeto

Para executar o programa, existem duas opções:

- Clonar o repositório e utilizar a CLI `dotnet`
- Baixar o [último release](https://github.com/arthurvergacas/desafio-broker/releases) do projeto no github

### Clonando o repositório

Na pasta em que desejar armazenar o projeto, clone o projeto utilizando o `git` e entre no repositório clonado:

```sh
git clone https://github.com/arthurvergacas/desafio-broker.git
cd desafio-broker
```

### Baixando o release

Navegue para a [página de releases do repositório no Github](https://github.com/arthurvergacas/desafio-broker/releases) e encontre a última versão.

Lá, você encontrará para download o arquivo `win10-x64.zip`. É nesta pasta zipada que se encontra o executável do programa.

Baixe o arquivo e abra-o. Dentro, você encontrará uma pasta contendo dois arquivos:

- `DesafioBroker.exe`: O executável do programa
- `appsettings.json`: O arquivo de configurações do programa

### Configurando

Antes de rodar o proejto é preciso configurá-lo através do arquivo `appsettings.json`. Há um arquivo de configuração modelo tanto no repositório quanto no arquivo de release, para que você possa modificá-lo mais facilmente.

**Se você clonou o repositório**, renomeie o arquivo `appsettings.example.json` para `appsettings.json`. Isto é importante para que o progama consiga encontrar o arquivo de configuração.

Abra o arquivo que você acabou de renomear e preencha os dados relevantes, como:

- O email que deseja receber as notificações, no campo `"Recipient"`
- A configuração do seu servidor SMTP, no campo `"SMTPConfig"`
- O intervalo entre as buscas de preço da sua ação na bolsa, **em segundos**, no campo `"FetchInterval"`. O intervalo padrão é de 5 minutos (300 segundos)

### Rodando o projeto

A forma de executar o projeto difere um pouco dependendo de como você o baixou.

#### Se você optou por clonar o repositório

Com um terminal na pasta `desafio-broker`, rode o seguinte comando:

```sh
dotnnet run --project .\DesafioBroker\ SIMBOLO_DA_ACAO VALOR_DE_COMPRA VALOR_DE_VENDA
```

Por exemplo:

```sh
dotnnet run --project .\DesafioBroker\ PETR4 23.4 25.62
```

#### Se você optou por baixar o release

Com um terminal na pasta em que se encontram o executável e o arquivo de configuração, rode o seguinte comando:

```sh
.\DesafioBroker.exe SIMBOLO_DA_ACAO VALOR_DE_COMPRA VALOR_DE_VENDA
```

Por exemplo:

```sh
.\DesafioBroker.exe PETR4 23.4 25.62
```
