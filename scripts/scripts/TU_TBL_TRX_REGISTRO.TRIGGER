CREATE OR REPLACE  TRIGGER LAFT.TU_TBL_TRX_REGISTRO
AFTER UPDATE
   ON LAFT.TBL_TRX_REGISTRO
   FOR EACH ROW
DECLARE
BEGIN

    INSERT INTO LAFT.TBL_HIS_REGISTRO(NID,NSEQ,SNUMERO,NIDCARGA,NIDPERSONA,SPERSONA,NIDPAIS,SPAIS,
                                NIDSENIAL,SSENIAL,NIDDOCUMENTO,SDOCUMENTO,SNUMDOC,
                                SAPEPAT,SAPEMAT,SNOMBRE,SOBSERVACION,DREGISTRO,SUSUARIO,NESTADO,NEDITADO,
                                SUSERDB,SUSERPC,SIP,SHOST,DREGHIS,STIPHIS)
    VALUES(:NEW.NID,:NEW.NSEQ,:NEW.SNUMERO,:NEW.NIDCARGA,:NEW.NIDPERSONA,:NEW.SPERSONA,:NEW.NIDPAIS,:NEW.SPAIS,
            :NEW.NIDSENIAL,:NEW.SSENIAL,:NEW.NIDDOCUMENTO,:NEW.SDOCUMENTO,:NEW.SNUMDOC,
            :NEW.SAPEPAT,:NEW.SAPEMAT,:NEW.SNOMBRE,:NEW.SOBSERVACION,:NEW.DREGISTRO,:NEW.SUSUARIO,:NEW.NESTADO,:NEW.NEDITADO,
            SYS_CONTEXT('USERENV','SESSION_USER'),SYS_CONTEXT('USERENV','OS_USER'),SYS_CONTEXT('USERENV','IP_ADDRESS'),SYS_CONTEXT('USERENV','HOST'),SYSDATE  ,'U');

END;
