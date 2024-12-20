﻿# AutomacaoDeTeste
Projeto de Automação de Testes para Lógica de PLC
Este é o primeiro projeto para a automação de testes de lógica de PLC. Utilizamos uma lógica simples desenvolvida no TIA Portal e exportada com a ferramenta auxiliar da Siemens, o TIA Openness.

Descrição do Projeto
No software, empregamos o paradigma de orientação a objetos. Os contatos foram extraídos do arquivo XML se paseando nas condiçoes da logica LADDER, incluindo:

NA (Normalmente Aberto)
NF (Normalmente Fechado)
Bobinas de Set
Bobinas de Reset
Bobinas de Memoria
Linha em serie ou parelelo

Com os objetos instanciados, reconstruímos a sequência do TIA Portal dentro do software. Este processo gera um arquivo Excel com as tags dos contatos, permitindo que o usuário escreva os valores que deseja simular. No final, o software preenche o valor do resultado na lógica.

Objetivo
O objetivo do software é automatizar todos os testes de um controlador, tentando detectar lógicas erradas onde uma determinada ação requerida não seja atingida, o que poderia travar o processo. Esta versão inicial ajuda a identificar o que precisa ser feito.

Próximos Passos
O objetivo final é desenvolver uma interface gráfica que permita testar quantos conjuntos de dados forem necessários de forma eficiente. Além disso, o software poderá executar testes automaticamente e fornecer sugestões de boas práticas, identificar partes do código que não são executadas e possíveis erros de lógica que possam causar falhas no controlador.

Como Utilizar
Configuração Inicial:

Desenvolva a lógica no TIA Portal.

Exporte a lógica utilizando o TIA Openness.

Execução do Software:

Execute o software para extrair os contatos do arquivo XML.

O software gerará um arquivo Excel com as tags dos contatos.

Simulação de Valores:

No arquivo Excel gerado, preencha os valores que deseja simular.

O software processará os valores e preencherá o resultado na lógica.
