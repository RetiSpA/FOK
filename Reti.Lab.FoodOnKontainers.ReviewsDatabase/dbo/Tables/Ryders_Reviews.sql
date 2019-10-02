CREATE TABLE [dbo].[Ryders_Reviews] (
    [id_order]   INT            NOT NULL,
    [id_ryder]   INT            NOT NULL,
    [ryder_name] NVARCHAR (50)  NOT NULL,
    [id_user]    INT            NOT NULL,
    [user_name]  NVARCHAR (50)  NOT NULL,
    [review]     NVARCHAR (MAX) NOT NULL,
    [rating]     SMALLINT       NOT NULL,
    CONSTRAINT [PK_Ryder_Reviews] PRIMARY KEY CLUSTERED ([id_order] ASC)
);

