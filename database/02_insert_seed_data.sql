USE db2025108032;

-- 1. Inserir Condições do Jogo
INSERT INTO tb_condicao_jogo (co_condicao_jogo, ds_condicao_jogo) VALUES
(1, 'Novo (Lacre de Fábrica)'),
(2, 'Usado - Como Novo'),
(3, 'Usado - Com Sinais de Uso'),
(4, 'Colecionador (Item Raro)');

-- 2. Inserir Perfis de Usuário
INSERT INTO tb_perfil (co_perfil, ds_perfil) VALUES
(1, 'Administrador'),
(2, 'Cliente');

-- 3. Inserir Plataformas
INSERT INTO tb_plataforma (no_plataforma) VALUES
('Super Nintendo (SNES)'),
('Mega Drive'),
('PlayStation 1'),
('Nintendo 64'),
('Game Boy Color');

-- 4. Inserir Gêneros
INSERT INTO tb_genero (no_genero) VALUES
('Plataforma'),
('RPG'),
('Ação/Aventura'),
('Corrida'),
('Luta');

-- 5. Inserir Jogos de Exemplo
INSERT INTO tb_jogo (co_plataforma, co_condicao_jogo, no_jogo, ds_jogo, vl_preco, qt_estoque, st_ativo) VALUES
(1, 2, 'Super Mario World', 'Clássico jogo de plataforma do SNES completo na caixa original.', 199.90, 0, TRUE),
(1, 4, 'Chrono Trigger', 'Cartucho original em excelente estado de conservação.', 850.00, 1, TRUE),
(2, 3, 'Sonic the Hedgehog 2', 'Cartucho para Mega Drive funcionando perfeitamente.', 89.90, 8, TRUE),
(3, 1, 'Castlevania: Symphony of the Night', 'Jogo lacrado de fábrica versão Black Label.', 1200.00, 1, TRUE),
(4, 2, 'The Legend of Zelda: Ocarina of Time', 'Fita dourada original N64 com manual.', 450.00, 3, TRUE);

-- 6. Relacionar Jogos aos Gêneros
INSERT INTO tb_jogo_genero (co_jogo, co_genero) VALUES
(1, 1),
(2, 2),
(3, 1),
(4, 2),
(4, 3),
(5, 3);

-- 7. Inserir Usuários de Teste
INSERT INTO tb_usuario (co_perfil, no_usuario, ds_email, ds_senha_hash, st_ativo) VALUES
(1, 'Admin RetroGames', 'admin@retrogames.com', 'hash_senha_admin_123', TRUE),
(2, 'Cliente Teste', 'cliente@email.com', 'hash_senha_cliente_123', TRUE);