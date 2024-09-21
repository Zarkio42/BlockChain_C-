# Blockchain APP

Este projeto é uma implementação simples de uma blockchain utilizando C#. Ele demonstra os conceitos fundamentais de blocos, hashing, mineração e validação de uma cadeia de blocos, permitindo a criação e verificação de uma blockchain.

## Funcionalidades

- **Blocos**: Cada bloco contém dados, um hash, o hash do bloco anterior, um valor nonce para mineração, e um timestamp.
- **Mineração**: Processo de ajuste do valor nonce até encontrar um hash que atenda a uma condição de dificuldade (número específico de zeros no início do hash).
- **Validação**: Verificação da integridade de cada bloco e de toda a cadeia de blocos.
- **Bloco Gênesis**: O primeiro bloco da cadeia é o bloco gênesis, que é minerado e inicializado com um hash especial.

## Tecnologias Utilizadas

- **C#**: Linguagem de programação utilizada para construir o projeto.
- **SHA256**: Algoritmo de hash seguro usado para garantir a integridade dos dados de cada bloco.
- **Estruturas de Dados**: Utilização de listas para armazenar e encadear blocos.

## Como Funciona

### 1. Estrutura do Bloco (Block)

Cada bloco na blockchain contém as seguintes propriedades:

- **Data**: Os dados do bloco, armazenados em um array de bytes.
- **Hash**: O hash gerado para o bloco usando SHA256, que depende dos dados, nonce, hash do bloco anterior e timestamp.
- **Nonce**: Um número que é ajustado durante a mineração até encontrar um hash que atenda à dificuldade.
- **PrevHash**: O hash do bloco anterior, que garante o encadeamento de blocos.
- **TimeStamp**: O momento em que o bloco foi criado.

### 2. Mineração

A mineração é o processo de encontrar um nonce que gere um hash que atenda à condição de dificuldade definida. Neste projeto, a dificuldade é um hash que deve começar com dois bytes `0x00`. O processo ajusta o valor do nonce até encontrar um hash válido.

```csharp
while (!hash.Take(2).SequenceEqual(difficulty))
{
    block.Nonce++;
    hash = block.GenerateHash();
}
```

## 3. Validação
Após a criação de cada novo bloco, a blockchain é validada para garantir que:

- **Cada bloco é válido:** O hash gerado a partir dos dados do bloco deve corresponder ao hash armazenado no bloco.
- **O bloco anterior está corretamente vinculado:** O hash do bloco anterior no bloco atual deve corresponder ao hash do bloco anterior na cadeia.

## 4. Bloco Gênesis
O primeiro bloco da blockchain, chamado de "bloco gênesis", é inicializado com um hash pré-definido e um valor de nonce inicializado em zero.

## 5. Interface e Extensões
- **Interface IBlock:** Define as propriedades e métodos básicos que um bloco deve implementar.
- **Classe Block:** Implementa a interface IBlock e fornece uma estrutura para blocos individuais.
- **Extensões de Bloco:** Métodos de extensão para gerar hashes, minerar blocos e validar a cadeia de blocos.
- **Classe BlockChain:** Gerencia a cadeia de blocos, incluindo a adição de novos blocos e a validação da cadeia.

## Executando o Projeto
Para executar o projeto:

1. Compile o código C#.
2. Execute o programa para gerar e validar a blockchain.
3. O programa cria 200 blocos e valida a cadeia, exibindo a saída no console.

Certifique-se de ter o .NET SDK instalado e configurado para compilar e executar o projeto.

### Exemplo de Execução
```csharp
var ran = new Random();
IBlock genesis = new Block(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00 });
byte[] difficulty = new byte[] { 0x00, 0x00 };
BlockChain chain = new BlockChain(difficulty, genesis);

for (int i = 0; i < 200; i++)
{
    var data = Enumerable.Range(0, 255).Select(p => (byte)ran.Next());
    chain.Add(new Block(data.ToArray()));
    Console.WriteLine(chain.LastOrDefault()?.ToString());
    if (chain.IsValid())
    {
        Console.WriteLine("Blockchain válida!");
    }
}

Console.ReadLine();
```

## [Vídeo exemplo](https://www.youtube.com/watch?v=V6lqdJPDzI0&list=PLXLkA7FAishohjZkwmPTACpCIXKvc7Fyl&index=4)

