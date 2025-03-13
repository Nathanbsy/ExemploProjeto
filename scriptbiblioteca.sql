CREATE DATABASE bdBiblioteca1902;
USE bdBiblioteca1902;

CREATE TABLE tbUsuario (
    idUsuario INT PRIMARY KEY AUTO_INCREMENT,
    nomeUsuario VARCHAR(50) NOT NULL,
    emailUsuario VARCHAR(80) UNIQUE NOT NULL,
    telUsuario VARCHAR(14),
    enderecoUsuario VARCHAR(255)
);

CREATE TABLE tbGenero (
    idGenero INT PRIMARY KEY AUTO_INCREMENT,
    nomeGenero VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE tbAutor (
    idAutor INT PRIMARY KEY AUTO_INCREMENT,
    nomeAutor VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE tbEditora (
    idEditora INT PRIMARY KEY AUTO_INCREMENT,
    nomeEditora VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE tbLivro (
    idLivro INT PRIMARY KEY AUTO_INCREMENT,
    nomeLivro VARCHAR(50) NOT NULL,
    idGenero INT NOT NULL,
    idAutor INT NOT NULL,
    idEditora INT NOT NULL,
    edicaoLivro VARCHAR(10),
    statusLivro ENUM('Disponível', 'Emprestado', 'Reservado') DEFAULT 'Disponível',
    FOREIGN KEY (idGenero) REFERENCES tbGenero(idGenero),
    FOREIGN KEY (idAutor) REFERENCES tbAutor(idAutor),
    FOREIGN KEY (idEditora) REFERENCES tbEditora(idEditora)
);

CREATE TABLE tbEmprestimo (
    idEmprestimo INT PRIMARY KEY AUTO_INCREMENT,
    idUsuario INT NOT NULL,
    dataEmprestimo DATE NOT NULL,
    dataDevolucao DATE,
    statusEmprestimo ENUM('Ativo', 'Finalizado', 'Atrasado') DEFAULT 'Ativo',
    FOREIGN KEY (idUsuario) REFERENCES tbUsuario(idUsuario)
);

CREATE TABLE tbLivroEmprestimo (
    idLivroEmprestimo INT PRIMARY KEY AUTO_INCREMENT,
    idLivro INT NOT NULL,
    idEmprestimo INT NOT NULL,
    FOREIGN KEY (idLivro) REFERENCES tbLivro(idLivro),
    FOREIGN KEY (idEmprestimo) REFERENCES tbEmprestimo(idEmprestimo)
);
