
\connect wishlistdb

CREATE TABLE usuario
(
    id serial PRIMARY KEY,
    email  VARCHAR (100)  NOT NULL,
    nome  VARCHAR (100)  NOT NULL
);

CREATE TABLE wishlist
(
    id serial PRIMARY KEY,
    id_usuario INT,
    titulo_produto VARCHAR (500)  NOT NULL,
    desc_produto VARCHAR (500),
    link_produto VARCHAR (100),
    foto_produto VARCHAR (100),
    comprou_ganhou_item BOOLEAN default false,

    CONSTRAINT fk_usuario
        FOREIGN KEY(id_usuario) 
	        REFERENCES usuario(id)
);

ALTER TABLE "usuario" OWNER TO wishlistuser;
ALTER TABLE "wishList" OWNER TO wishlistuser;

Insert into usuario(email, nome) values( 'guest', 'guest');
Insert into wishList(id_usuario, titulo_produto, desc_produto) values( 1, 'titulo-produto', 'descrição-produto');