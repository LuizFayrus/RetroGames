USE db2025108032;
-- 1. Condição do Jogo (Chave fixa, sem AUTO_INCREMENT)
CREATE TABLE tb_condicao_jogo (
    co_condicao_jogo INT NOT NULL,
    ds_condicao_jogo VARCHAR(50) NOT NULL,
    CONSTRAINT pk_tb_condicao_jogo PRIMARY KEY (co_condicao_jogo),
    CONSTRAINT uq_tb_condicao_jogo_ds UNIQUE (ds_condicao_jogo)
);

-- 2. Perfil (Chave fixa, sem AUTO_INCREMENT)
CREATE TABLE tb_perfil (
    co_perfil INT NOT NULL,
    ds_perfil VARCHAR(50) NOT NULL,
    CONSTRAINT pk_tb_perfil PRIMARY KEY (co_perfil),
    CONSTRAINT uq_tb_perfil_ds UNIQUE (ds_perfil)
);

-- 3. Plataforma
CREATE TABLE tb_plataforma (
    co_seq_plataforma INT AUTO_INCREMENT NOT NULL,
    no_plataforma VARCHAR(100) NOT NULL,
    CONSTRAINT pk_tb_plataforma PRIMARY KEY (co_seq_plataforma),
    CONSTRAINT uq_tb_plataforma_no UNIQUE (no_plataforma)
);

-- 4. Gênero
CREATE TABLE tb_genero (
    co_seq_genero INT AUTO_INCREMENT NOT NULL,
    no_genero VARCHAR(100) NOT NULL,
    CONSTRAINT pk_tb_genero PRIMARY KEY (co_seq_genero),
    CONSTRAINT uq_tb_genero_no UNIQUE (no_genero)
);

-- 5. Jogo
CREATE TABLE tb_jogo (
    co_seq_jogo INT AUTO_INCREMENT NOT NULL,
    co_plataforma INT NOT NULL,
    co_condicao_jogo INT NOT NULL,
    no_jogo VARCHAR(150) NOT NULL,
    ds_jogo TEXT NULL,
    vl_preco DECIMAL(10,2) NOT NULL,
    qt_estoque INT NOT NULL DEFAULT 0,
    st_ativo BOOLEAN NOT NULL DEFAULT TRUE,
    dt_cadastro DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT pk_tb_jogo PRIMARY KEY (co_seq_jogo),
    CONSTRAINT fk_tb_jogo_plataforma FOREIGN KEY (co_plataforma) REFERENCES tb_plataforma (co_seq_plataforma),
    CONSTRAINT fk_tb_jogo_condicao FOREIGN KEY (co_condicao_jogo) REFERENCES tb_condicao_jogo (co_condicao_jogo),
    INDEX idx_tb_jogo_no (no_jogo)
);

-- 6. Jogo Gênero (Tabela associativa)
CREATE TABLE tb_jogo_genero (
    co_jogo INT NOT NULL,
    co_genero INT NOT NULL,
    CONSTRAINT pk_tb_jogo_genero PRIMARY KEY (co_jogo, co_genero),
    CONSTRAINT fk_tb_jogo_genero_jogo FOREIGN KEY (co_jogo) REFERENCES tb_jogo (co_seq_jogo) ON DELETE CASCADE,
    CONSTRAINT fk_tb_jogo_genero_genero FOREIGN KEY (co_genero) REFERENCES tb_genero (co_seq_genero)
);

-- 7. Usuário
CREATE TABLE tb_usuario (
    co_seq_usuario INT AUTO_INCREMENT NOT NULL,
    co_perfil INT NOT NULL,
    no_usuario VARCHAR(150) NOT NULL,
    ds_email VARCHAR(150) NOT NULL,
    ds_senha_hash VARCHAR(255) NOT NULL,
    st_ativo BOOLEAN NOT NULL DEFAULT TRUE,
    dt_cadastro DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
    CONSTRAINT pk_tb_usuario PRIMARY KEY (co_seq_usuario),
    CONSTRAINT fk_tb_usuario_perfil FOREIGN KEY (co_perfil) REFERENCES tb_perfil (co_perfil),
    CONSTRAINT uq_tb_usuario_email UNIQUE (ds_email)
);