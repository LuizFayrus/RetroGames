USE db2025108032;

-- ============================================================
-- PARTE 1: CONSULTAS
-- ============================================================

-- 1. Catálogo: jogos ativos com nome da plataforma e descrição da condição, ordenados por nome
SELECT 
    j.no_jogo,
    p.no_plataforma,
    c.ds_condicao_jogo,
    j.vl_preco,
    j.qt_estoque
FROM tb_jogo j
INNER JOIN tb_plataforma p ON j.co_plataforma = p.co_seq_plataforma
INNER JOIN tb_condicao_jogo c ON j.co_condicao_jogo = c.co_condicao_jogo
WHERE j.st_ativo = TRUE
ORDER BY j.no_jogo ASC;

-- 2. Gêneros de um jogo informado (Exemplo: co_jogo = 4 - Castlevania)
SELECT 
    j.no_jogo,
    g.no_genero
FROM tb_jogo_genero jg
INNER JOIN tb_jogo j ON jg.co_jogo = j.co_seq_jogo
INNER JOIN tb_genero g ON jg.co_genero = g.co_seq_genero
WHERE jg.co_jogo = 4;

-- 3. Jogos de um gênero informado (Exemplo: co_genero = 1 - Plataforma)
SELECT 
    g.no_genero,
    j.no_jogo
FROM tb_jogo_genero jg
INNER JOIN tb_jogo j ON jg.co_jogo = j.co_seq_jogo
INNER JOIN tb_genero g ON jg.co_genero = g.co_seq_genero
WHERE jg.co_genero = 1;

-- 4. Quantidade de jogos por plataforma, incluindo plataformas sem jogos
SELECT 
    p.no_plataforma,
    COUNT(j.co_seq_jogo) AS total_jogos
FROM tb_plataforma p
LEFT JOIN tb_jogo j ON p.co_seq_plataforma = j.co_plataforma
GROUP BY p.co_seq_plataforma, p.no_plataforma;

-- 5. Busca por parte do nome do jogo, com 5 itens por página (Página 1: LIMIT 5 OFFSET 0)
SELECT * 
FROM tb_jogo
WHERE no_jogo LIKE '%a%'
ORDER BY no_jogo ASC
LIMIT 5 OFFSET 0;


-- ============================================================
-- PARTE 2: TESTES DE INTEGRIDADE
-- ============================================================

-- Teste 1: Inserir jogo com co_plataforma inexistente
-- Resultado esperado / obtido: Erro 1452 (chave estrangeira)
-- INSERT INTO tb_jogo (co_plataforma, co_condicao_jogo, no_jogo, vl_preco, qt_estoque) VALUES (999, 1, 'Jogo Inexistente', 100.00, 1);

-- Teste 2: Inserir plataforma com nome já cadastrado
-- Resultado esperado / obtido: Erro 1062 (UNIQUE)
-- INSERT INTO tb_plataforma (no_plataforma) VALUES ('Super Nintendo (SNES)');

-- Teste 3: Excluir plataforma que possui jogos
-- Resultado esperado / obtido: Erro 1451 (chave estrangeira)
-- DELETE FROM tb_plataforma WHERE co_seq_plataforma = 1;

-- Teste 4: Inserir jogo com no_jogo nulo
-- Resultado esperado / obtido: Erro 1048 (NOT NULL)
-- INSERT INTO tb_jogo (co_plataforma, co_condicao_jogo, no_jogo, vl_preco, qt_estoque) VALUES (1, 1, NULL, 100.00, 1);

-- Teste 5: Inserir jogo com vl_preco igual a 0
-- Resultado esperado / obtido: Aceito (Regra fica no Service RN06)
-- INSERT INTO tb_jogo (co_plataforma, co_condicao_jogo, no_jogo, vl_preco, qt_estoque) VALUES (1, 1, 'Jogo Grátis Teste', 0.00, 1);
-- DELETE FROM tb_jogo WHERE no_jogo = 'Jogo Grátis Teste';