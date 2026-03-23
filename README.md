<h1 align="center" style="color:#c2559c;">Biblioteca Rosa</h1>
<br/>
<p align="center">
<span style="color:#c2559c;font-weight:bold"> Biblioteca Rosa </span> é uma API REST de controle básico de uma biblioteca universitária, afim de automatizar as tarefas básicas do dia a dia de uma biblioteca, como controle de aluguel e devolução de livros, cadastro de usuários, garantindo integridade da biblioteca
</p>

## Regras de negócio
### Empréstimos
- Alunos podem ter até 5 livros simultaneamente
- Professores podem ter até 5 livros simultaneamente
- O prazo de empréstimo para alunos é de 10 dias corridos
- O prazo de empréstimo para professores é de 1 mês corrido
- O empréstimo só pode ser realizado se houver exemplares disponíveis
- Um usuário não pode emprestar o mesmo livro mais de uma vez ao mesmo tempo

### Renovação
- O empréstimo pode ser renovado até 3 vezes
- A renovação só é permitida antes da data de vencimento
- Não é possível renovar se o livro estiver reservado por outro usuário

### Reservas
- Usuários não podem reservar livros indisponíveis
- O sistema deve manter uma fila de espera
- Quando o livro for devolvido, ele fica disponível para o próximo da fila

### Atrasos
- 1 semana de atraso, corresponde a 2 semanas sem novos empréstimos
- 1 dia de atraso, corresponde a mais 1 dia sem novos empréstimos
- Não serão aceitos pagamentos de multa

### Restrições
- Apenas administradores e bibliotecários podem cadastrar, editar ou remover livros

### Histórico
- Todos os empréstimos devem ser armazenados
- O histórico não pode ser apagado

## Alunos envolvidos
1221141558 - Lucas Figueiredo de Almeida Castilho Soares\
125111410617 - Livia Steise Gaspar Diniz\
125111382859 - Augusto Felipe de Paula Coimbra\
125111385813 - Bernardo de Paula Dias\
125111401298 - Henrique Márcio Dias Alves\
125111404838 - Luiz Guilherme Vilaça de Moraes