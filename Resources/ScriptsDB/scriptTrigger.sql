CREATE TRIGGER TR_BeforeDelete
ON Clothes
INSTEAD OF DELETE
AS
BEGIN
    
    INSERT INTO ClothesDeleted(id, name, idColor, idSize, price, description, dateDeleted)
    SELECT id, name, idColor, idSize, price, description, GETDATE()
    FROM deleted;
    
    -- Eliminar los registros de la tabla original
    DELETE FROM Clothes
    WHERE ID IN (SELECT ID FROM deleted);
END;