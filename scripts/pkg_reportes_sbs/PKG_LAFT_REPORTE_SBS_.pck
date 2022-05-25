CREATE OR REPLACE PACKAGE PKG_LAFT_REPORTE_SBS_ IS

  TYPE MYCURSOR IS REF CURSOR;
  TYPE MYCURSOR2 IS REF CURSOR;
  PROCEDURE SINIESTROS_MAYORES(P_OPE    NUMBER,
                               P_TC     NUMBER,
                               P_MONTO  NUMBER,
                               P_FECINI VARCHAR2,
                               P_FECFIN VARCHAR2,
                               C_TABLE  OUT MYCURSOR);

  PROCEDURE RENTAS_MAYORES(P_FECINI      VARCHAR2,
                           P_FECFIN      VARCHAR2,
                           P_MONTO_UNICO NUMBER,
                           P_TC          NUMBER,
                           C_TABLE       OUT MYCURSOR);

  PROCEDURE PRIMAS_MAYORES_UNICA(P_OPE    NUMBER,
                                 P_TC     NUMBER,
                                 P_MONTO  NUMBER,
                                 P_FECINI VARCHAR2,
                                 P_FECFIN VARCHAR2,
                                 C_TABLE  OUT MYCURSOR);

  PROCEDURE PRIMAS_MAYORES_MULTIPLE(P_OPE    NUMBER,
                                    P_TC     NUMBER,
                                    P_MONTO  NUMBER,
                                    P_FECINI VARCHAR2,
                                    P_FECFIN VARCHAR2,
                                    C_TABLE  OUT MYCURSOR);

  PROCEDURE VIDA_MAYORES(P_OPE    NUMBER,
                         P_TC     NUMBER,
                         P_MONTO  NUMBER,
                         P_FECINI VARCHAR2,
                         P_FECFIN VARCHAR2,
                         C_TABLE  OUT MYCURSOR);

  PROCEDURE NC_VIDA_MAYORES(P_OPE    NUMBER,
                            P_TC     NUMBER,
                            P_MONTO  NUMBER,
                            P_FECINI VARCHAR2,
                            P_FECFIN VARCHAR2,
                            C_TABLE  OUT MYCURSOR);

  PROCEDURE NC_VIDA_MAYORES_2(P_OPE    NUMBER,
                              P_TC     NUMBER,
                              P_MONTO  NUMBER,
                              P_FECINI VARCHAR2,
                              P_FECFIN VARCHAR2,
                              C_TABLE  OUT MYCURSOR);

  PROCEDURE COMISIONES(P_OPE    NUMBER,
                       P_TC     NUMBER,
                       P_MONTO  NUMBER,
                       P_FECINI VARCHAR2,
                       P_FECFIN VARCHAR2,
                       C_TABLE  OUT MYCURSOR);

  PROCEDURE SPS_OBT_CORREO_ENVIO(P_USUARIO  NUMBER,
                                 P_SEMAIL   OUT VARCHAR2,
                                 P_SNAME    OUT VARCHAR2,
                                 P_NCODE    OUT NUMBER,
                                 P_SMESSAGE OUT VARCHAR2);

  /*PROCEDURE GENERAR_REP_SBS(P_OPE          NUMBER,
                            P_TC           NUMBER,
                            P_MONTO        NUMBER,
                            P_FECINI       VARCHAR2,
                            P_FECFIN       VARCHAR2,
                            P_EST_REPORTES VARCHAR2,
                            P_TIPO_ARCHIVO VARCHAR2,
                            P_SRUTA        OUT VARCHAR2,
                            P_ID_REPORTE   OUT VARCHAR2,
                            P_NCODE        OUT NUMBER,
                            P_SMESSAGE     OUT VARCHAR2,
                            C_TABLE        OUT SYS_REFCURSOR);*/

  PROCEDURE SP_OBT_TIPO_CAMBIO(P_TIPOCAMBIO OUT NUMBER);

  PROCEDURE SP_OBT_RUTA(P_ID   VARCHAR2,
                        P_RUTA OUT VARCHAR2);

  /*PROCEDURE SP_MONITOREO_REP_SBS(P_FECEJE_INI  VARCHAR2,
                                 P_FECEJE_FIN  VARCHAR2,
                                 P_SID         VARCHAR,
                                 P_TI_BUSQUEDA VARCHAR2,
                                 P_NCODE       OUT NUMBER,
                                 P_SMESSAGE    OUT VARCHAR2,
                                 C_TABLE       OUT SYS_REFCURSOR);*/
                                 
PROCEDURE SINIESTROS_MAYORES_CON(P_OPE    NUMBER,
                               P_TC     NUMBER,
                               P_MONTO  NUMBER,
                               P_FECINI VARCHAR2,
                               P_FECFIN VARCHAR2,
                               C_TABLE  OUT MYCURSOR);                              

END PKG_LAFT_REPORTE_SBS_;
/
CREATE OR REPLACE PACKAGE BODY PKG_LAFT_REPORTE_SBS_ AS

  /*Variables comunes al paquete*/
  V_FILA             VARCHAR2(1000);
  V_OFICINA          VARCHAR2(1000);
  V_OPERACION        VARCHAR2(1000);
  V_INTERNO          VARCHAR2(1000);
  V_MODALIDAD        VARCHAR2(1000);
  V_OPE_UBIGEO       VARCHAR2(1000);
  V_OPE_FECHA        VARCHAR2(1000);
  V_OPE_HORA         VARCHAR2(1000);
  V_EJE_RELACION     VARCHAR2(1000);
  V_EJE_CONDICION    VARCHAR2(1000);
  V_EJE_TIPPER       VARCHAR2(1000);
  V_EJE_TIPDOC       VARCHAR2(1000);
  V_EJE_NUMDOC       VARCHAR2(1000);
  V_EJE_NUMRUC       VARCHAR2(1000);
  V_EJE_APEPAT       VARCHAR2(1000);
  V_EJE_APEMAT       VARCHAR2(1000);
  V_EJE_NOMBRES      VARCHAR2(1000);
  V_EJE_OCUPACION    VARCHAR2(1000);
  V_EJE_PAIS         VARCHAR2(1000);
  V_EJE_CARGO        VARCHAR2(1000);
  V_EJE_PEP          VARCHAR2(1000);
  V_EJE_DOMICILIO    VARCHAR2(1000);
  V_EJE_DEPART       VARCHAR2(1000);
  V_EJE_PROV         VARCHAR2(1000);
  V_EJE_DIST         VARCHAR2(1000);
  V_EJE_TELEFONO     VARCHAR2(1000);
  V_ORD_RELACION     VARCHAR2(1000);
  V_ORD_CONDICION    VARCHAR2(1000);
  V_ORD_TIPPER       VARCHAR2(1000);
  V_ORD_TIPDOC       VARCHAR2(1000);
  V_ORD_NUMDOC       VARCHAR2(1000);
  V_ORD_NUMRUC       VARCHAR2(1000);
  V_ORD_APEPAT       VARCHAR2(1000);
  V_ORD_APEMAT       VARCHAR2(1000);
  V_ORD_NOMBRES      VARCHAR2(1000);
  V_ORD_OCUPACION    VARCHAR2(1000);
  V_ORD_PAIS         VARCHAR2(1000);
  V_ORD_CARGO        VARCHAR2(1000);
  V_ORD_PEP          VARCHAR2(1000);
  V_ORD_DOMICILIO    VARCHAR2(1000);
  V_ORD_DEPART       VARCHAR2(1000);
  V_ORD_PROV         VARCHAR2(1000);
  V_ORD_DIST         VARCHAR2(1000);
  V_ORD_TELEFONO     VARCHAR2(1000);
  V_BEN_RELACION     VARCHAR2(1000);
  V_BEN_CONDICION    VARCHAR2(1000);
  V_BEN_TIP_PER      VARCHAR2(1000);
  V_BEN_TIP_DOC      VARCHAR2(1000);
  V_BEN_NUM_DOC      VARCHAR2(1000);
  V_BEN_NUM_RUC      VARCHAR2(1000);
  V_BEN_APEPAT       VARCHAR2(1000);
  V_BEN_APEMAT       VARCHAR2(1000);
  V_BEN_NOMBRES      VARCHAR2(1000);
  V_BEN_OCUPACION    VARCHAR2(1000);
  V_BEN_PAIS         VARCHAR2(1000);
  V_BEN_CARGO        VARCHAR2(1000);
  V_BEN_PEP          VARCHAR2(1000);
  V_BEN_DOMICILIO    VARCHAR2(1000);
  V_BEN_DEPART       VARCHAR2(1000);
  V_BEN_PROV         VARCHAR2(1000);
  V_BEN_DIST         VARCHAR2(1000);
  V_BEN_TELEFONO     VARCHAR2(1000);
  V_DAT_TIPFON       VARCHAR2(1000);
  V_DAT_TIPOPE       VARCHAR2(1000);
  V_DAT_DESOPE       VARCHAR2(1000);
  V_DAT_ORIFON       VARCHAR2(1000);
  V_DAT_MONOPE       VARCHAR2(1000);
  V_DAT_MONOPE_A     VARCHAR2(1000);
  V_DAT_MTOOPE       VARCHAR2(1000);
  V_DAT_MTOOPEA      VARCHAR2(1000);
  V_DAT_COD_ENT_INVO VARCHAR2(1000);
  V_DAT_COD_TIP_CTAO VARCHAR2(1000);
  V_DAT_COD_CTAO     VARCHAR2(1000);
  V_DAT_ENT_FNC_EXTO VARCHAR2(1000);
  V_DAT_COD_ENT_INVB VARCHAR2(1000);
  V_DAT_COD_TIP_CTAB VARCHAR2(1000);
  V_DAT_COD_CTAB     VARCHAR2(1000);
  V_DAT_ENT_FNC_EXTB VARCHAR2(1000);
  V_DAT_ALCANCE      VARCHAR2(1000);
  V_DAT_COD_PAISO    VARCHAR2(1000);
  V_DAT_COD_PAISD    VARCHAR2(1000);
  V_DAT_INTOPE       VARCHAR2(1000);
  V_DAT_FORMA        VARCHAR2(1000);
  V_DAT_INFORM       VARCHAR2(1000);
  V_ORIGEN           VARCHAR2(1000);
  V_ORD_DIRECCION    VARCHAR2(1000);
  V_UBIGEO           VARCHAR2(1000);
  V_BEN_TIPPER       VARCHAR2(1000);
  V_BEN_TIPDOC       VARCHAR2(1000);
  V_BEN_NUMDOC       VARCHAR2(1000);
  V_BEN_NUMRUC       VARCHAR2(1000);
  V_DAT_MONOPEA      VARCHAR2(1000);
  V_DAT_CODPAISO     VARCHAR2(1000);
  V_DAT_CODPAISD     VARCHAR2(1000);
  V_DAT_INTERMOPE    VARCHAR2(1000);
  V_TIPO             VARCHAR2(1000);

  -- prototipos de funciones y procedimientos privados
  PROCEDURE SETEAR_VARIABLES;
  -- prototipos de funciones y procedimientos privados

  PROCEDURE SETEAR_VARIABLES IS
  BEGIN
    V_FILA             := '';
    V_OFICINA          := '';
    V_OPERACION        := '';
    V_INTERNO          := '';
    V_MODALIDAD        := '';
    V_OPE_UBIGEO       := '';
    V_OPE_FECHA        := '';
    V_OPE_HORA         := '';
    V_EJE_RELACION     := '';
    V_EJE_CONDICION    := '';
    V_EJE_TIPPER       := '';
    V_EJE_TIPDOC       := '';
    V_EJE_NUMDOC       := '';
    V_EJE_NUMRUC       := '';
    V_EJE_APEPAT       := '';
    V_EJE_APEMAT       := '';
    V_EJE_NOMBRES      := '';
    V_EJE_OCUPACION    := '';
    V_EJE_PAIS         := '';
    V_EJE_CARGO        := '';
    V_EJE_PEP          := '';
    V_EJE_DOMICILIO    := '';
    V_EJE_DEPART       := '';
    V_EJE_PROV         := '';
    V_EJE_DIST         := '';
    V_EJE_TELEFONO     := '';
    V_ORD_RELACION     := '';
    V_ORD_CONDICION    := '';
    V_ORD_TIPPER       := '';
    V_ORD_TIPDOC       := '';
    V_ORD_NUMDOC       := '';
    V_ORD_NUMRUC       := '';
    V_ORD_APEPAT       := '';
    V_ORD_APEMAT       := '';
    V_ORD_NOMBRES      := '';
    V_ORD_OCUPACION    := '';
    V_ORD_PAIS         := '';
    V_ORD_CARGO        := '';
    V_ORD_PEP          := '';
    V_ORD_DOMICILIO    := '';
    V_ORD_DEPART       := '';
    V_ORD_PROV         := '';
    V_ORD_DIST         := '';
    V_ORD_TELEFONO     := '';
    V_BEN_RELACION     := '';
    V_BEN_CONDICION    := '';
    V_BEN_TIPPER       := '';
    V_BEN_TIPDOC       := '';
    V_BEN_TIP_PER      := '';
    V_BEN_TIP_DOC      := '';
    V_BEN_NUM_DOC      := '';
    V_BEN_NUM_RUC      := '';
    V_BEN_APEPAT       := '';
    V_BEN_APEMAT       := '';
    V_BEN_NOMBRES      := '';
    V_BEN_OCUPACION    := '';
    V_BEN_PAIS         := '';
    V_BEN_CARGO        := '';
    V_BEN_PEP          := '';
    V_BEN_DOMICILIO    := '';
    V_BEN_DEPART       := '';
    V_BEN_PROV         := '';
    V_BEN_DIST         := '';
    V_BEN_TELEFONO     := '';
    V_DAT_TIPFON       := '';
    V_DAT_TIPOPE       := '';
    V_DAT_DESOPE       := '';
    V_DAT_ORIFON       := '';
    V_DAT_MONOPE       := '';
    V_DAT_MONOPE_A     := '';
    V_DAT_MTOOPE       := '';
    V_DAT_MTOOPEA      := '';
    V_DAT_COD_ENT_INVO := '';
    V_DAT_COD_TIP_CTAO := '';
    V_DAT_COD_CTAO     := '';
    V_DAT_ENT_FNC_EXTO := '';
    V_DAT_COD_ENT_INVB := '';
    V_DAT_COD_TIP_CTAB := '';
    V_DAT_COD_CTAB     := '';
    V_DAT_ENT_FNC_EXTB := '';
    V_DAT_ALCANCE      := '';
    V_DAT_COD_PAISO    := '';
    V_DAT_COD_PAISD    := '';
    V_DAT_INTOPE       := '';
    V_DAT_FORMA        := '';
    V_DAT_INFORM       := '';
    V_ORIGEN           := '';
    V_ORD_DIRECCION    := '';
    V_UBIGEO           := '';
    V_BEN_NUMDOC       := '';
    V_BEN_NUMRUC       := '';
    V_DAT_MONOPEA      := '';
    V_DAT_CODPAISO     := '';
    V_DAT_CODPAISD     := '';
    V_TIPO             := '';

  END;
  --------------------------------
  --------------------------------

  PROCEDURE SINIESTROS_MAYORES(P_OPE    NUMBER,
                               P_TC     NUMBER,
                               P_MONTO  NUMBER,
                               P_FECINI VARCHAR2,
                               P_FECFIN VARCHAR2,
                               C_TABLE  OUT MYCURSOR) IS

    V_SCLIENT_ORD CLIENT.SCLIENT%TYPE;
    V_SPHONE_ORD  PHONES.SPHONE%TYPE;

    V_SCLIENT_AD        CLIENT.SCLIENT%TYPE;
    V_SDIRECCION_ORD    ADDRESS_CLIENT.SDESDIREBUSQ%TYPE;
    V_STI_DIRE          ADDRESS_CLIENT.STI_DIRE%TYPE;
    V_SNOM_DIRECCION    ADDRESS_CLIENT.SNOM_DIRECCION%TYPE;
    V_SNUM_DIRECCION    ADDRESS_CLIENT.SNUM_DIRECCION%TYPE;
    V_STI_BLOCKCHALET   ADDRESS_CLIENT.STI_BLOCKCHALET%TYPE;
    V_SBLOCKCHALET      ADDRESS_CLIENT.SBLOCKCHALET%TYPE;
    V_STI_INTERIOR      ADDRESS_CLIENT.STI_INTERIOR%TYPE;
    V_SNUM_INTERIOR     ADDRESS_CLIENT.SNUM_INTERIOR%TYPE;
    V_STI_CJHT          ADDRESS_CLIENT.STI_CJHT%TYPE;
    V_SNOM_CJHT         ADDRESS_CLIENT.SNOM_CJHT%TYPE;
    V_SETAPA            ADDRESS_CLIENT.SETAPA%TYPE;
    V_SMANZANA          ADDRESS_CLIENT.SMANZANA%TYPE;
    V_SLOTE             ADDRESS_CLIENT.SLOTE%TYPE;
    V_SREFERENCIA       ADDRESS_CLIENT.SREFERENCIA%TYPE;
    V_NMUNICIPALITY_ORD ADDRESS.NMUNICIPALITY%TYPE;
    V_NPROVINCE_ORD     PROVINCE.NPROVINCE%TYPE;
    V_NLOCAL_ORD        TAB_LOCAT.NLOCAL%TYPE;
    V_SCLIENAME_ORD     CLIENT.SCLIENAME%TYPE;
    V_SIDDOC_ORD        CLIENT_IDDOC.SIDDOC%TYPE;
    V_OFICINA           VARCHAR2(4);
    V_OPE_UBIGEO        VARCHAR2(6);
    V_ORD_RELACION      VARCHAR2(1);
    V_ORD_CONDICION     VARCHAR2(1);
    V_ORD_TIPPER        VARCHAR2(1);
    V_ORD_PAIS          VARCHAR2(2);
    V_BEN_RELACION      VARCHAR2(1);
    V_BEN_CONDICION     VARCHAR2(1);
    V_BEN_PAIS          VARCHAR2(2);
    V_DAT_TIPFON        VARCHAR2(1);
    V_DAT_TIPOPE        VARCHAR2(2);
    V_DAT_ALCANCE       VARCHAR2(1);
    V_DAT_FORMA         VARCHAR2(1);
    V_DISTRITO_ORD      MUNICIPALITY.SDESCRIPT%TYPE;
    V_PROVINCIA_ORD     TAB_LOCAT.SDESCRIPT%TYPE;
    V_DEPARTAMENTO_ORD  PROVINCE.SDESCRIPT%TYPE;
    V_ORD_TIPDOC        VARCHAR2(2); --CLIENT_IDDOC.NIDDOC_TYPE%TYPE;
    --V_DATA_INTERM VARCHAR2(1);

  BEGIN

    /*SELECT MAX(SVALOR)
    INTO V_SCLIENT_ORD
    FROM LAFT.TBL_CONFIG_REPORTES
    WHERE SORIGEN='SIN' AND SDESCAMPO='V_SCLIENT_ORD';*/

    SELECT SVALOR
      INTO V_OFICINA
      FROM LAFT.TBL_CONFIG_REPORTES
     WHERE SORIGEN = 'SIN'
       AND SDESCAMPO = 'OFICINA';

    SELECT SVALOR
      INTO V_OPE_UBIGEO
      FROM LAFT.TBL_CONFIG_REPORTES
     WHERE SORIGEN = 'SIN'
       AND SDESCAMPO = 'OPE_UBIGEO';

    SELECT SVALOR
      INTO V_ORD_RELACION
      FROM LAFT.TBL_CONFIG_REPORTES
     WHERE SORIGEN = 'SIN'
       AND SDESCAMPO = 'ORD_RELACION';

    SELECT SVALOR
      INTO V_ORD_CONDICION
      FROM LAFT.TBL_CONFIG_REPORTES
     WHERE SORIGEN = 'SIN'
       AND SDESCAMPO = 'ORD_CONDICION';

    SELECT SVALOR
      INTO V_ORD_TIPPER
      FROM LAFT.TBL_CONFIG_REPORTES
     WHERE SORIGEN = 'SIN'
       AND SDESCAMPO = 'ORD_TIPPER';

    SELECT SVALOR
      INTO V_ORD_PAIS
      FROM LAFT.TBL_CONFIG_REPORTES
     WHERE SORIGEN = 'SIN'
       AND SDESCAMPO = 'ORD_PAIS';

    SELECT SVALOR
      INTO V_BEN_RELACION
      FROM LAFT.TBL_CONFIG_REPORTES
     WHERE SORIGEN = 'SIN'
       AND SDESCAMPO = 'BEN_RELACION';

    SELECT SVALOR
      INTO V_BEN_CONDICION
      FROM LAFT.TBL_CONFIG_REPORTES
     WHERE SORIGEN = 'SIN'
       AND SDESCAMPO = 'BEN_CONDICION';

    SELECT SVALOR
      INTO V_BEN_PAIS
      FROM LAFT.TBL_CONFIG_REPORTES
     WHERE SORIGEN = 'SIN'
       AND SDESCAMPO = 'BEN_PAIS';

    SELECT SVALOR
      INTO V_DAT_TIPFON
      FROM LAFT.TBL_CONFIG_REPORTES
     WHERE SORIGEN = 'SIN'
       AND SDESCAMPO = 'DAT_TIPFON';

    SELECT SVALOR
      INTO V_DAT_TIPOPE
      FROM LAFT.TBL_CONFIG_REPORTES
     WHERE SORIGEN = 'SIN'
       AND SDESCAMPO = 'DAT_TIPOPE';

    SELECT SVALOR
      INTO V_DAT_ALCANCE
      FROM LAFT.TBL_CONFIG_REPORTES
     WHERE SORIGEN = 'SIN'
       AND SDESCAMPO = 'DAT_ALCANCE';

    SELECT SVALOR
      INTO V_DAT_FORMA
      FROM LAFT.TBL_CONFIG_REPORTES
     WHERE SORIGEN = 'SIN'
       AND SDESCAMPO = 'DAT_FORMA';

    /*SELECT SVALOR INTO V_DATA_INTERM FROM LAFT.TBL_CONFIG_REPORTES
    WHERE SORIGEN='SIN' AND SDESCAMPO='DAT_INTOPE';*/

    V_SCLIENT_ORD := '01020517207331';

    BEGIN

      SELECT /*+INDEX (AD XIF2110ADDRESS)*/
      --TRIM(CL.SCLIENAME),
      --TRIM(CI.SIDDOC),
       CL.SCLIENAME,
       CI.SCLINUMDOCU,
       ADC.STI_DIRE,
       ADC.SNOM_DIRECCION,
       ADC.SNUM_DIRECCION,
       ADC.STI_BLOCKCHALET,
       ADC.SBLOCKCHALET,
       ADC.STI_INTERIOR,
       ADC.SNUM_INTERIOR,
       ADC.STI_CJHT,
       ADC.SNOM_CJHT,
       ADC.SETAPA,
       ADC.SMANZANA,
       ADC.SLOTE,
       ADC.SREFERENCIA,
       TRIM(AD.SSTREET) || TRIM(AD.SSTREET1),
       ADC.SCLIENT,
       AD.NMUNICIPALITY,
       C.SDESCRIPT,
       D.SDESCRIPT,
       E.SDESCRIPT,
       CI.NTYPCLIENTDOC

        INTO V_SCLIENAME_ORD,
             V_SIDDOC_ORD,
             V_STI_DIRE,
             V_SNOM_DIRECCION,
             V_SNUM_DIRECCION,
             V_STI_BLOCKCHALET,
             V_SBLOCKCHALET,
             V_STI_INTERIOR,
             V_SNUM_INTERIOR,
             V_STI_CJHT,
             V_SNOM_CJHT,
             V_SETAPA,
             V_SMANZANA,
             V_SLOTE,
             V_SREFERENCIA,
             V_SDIRECCION_ORD,
             V_SCLIENT_AD,
             V_NMUNICIPALITY_ORD,
             V_DEPARTAMENTO_ORD,
             V_DISTRITO_ORD,
             V_PROVINCIA_ORD,
             V_ORD_TIPDOC

        FROM ADDRESS        AD,
             ADDRESS_CLIENT ADC,
             CLIENT         CL,
             CLIDOCUMENTS   CI,
             PROVINCE       C,
             TAB_LOCAT      D,
             MUNICIPALITY   E
       WHERE AD.SCLIENT = V_SCLIENT_ORD
         AND CL.SCLIENT = AD.SCLIENT
         AND CI.SCLIENT(+) = AD.SCLIENT
         AND AD.NRECOWNER = 2
         AND AD.SRECTYPE = 2
         AND AD.DNULLDATE IS NULL
         AND ADC.SCLIENT(+) = AD.SCLIENT
         AND ADC.NRECOWNER(+) = AD.NRECOWNER
         AND ADC.SKEYADDRESS(+) = AD.SKEYADDRESS
         AND ADC.DEFFECDATE(+) = AD.DEFFECDATE
         AND ADC.SRECTYPE(+) = AD.SRECTYPE
         AND E.NMUNICIPALITY = AD.NMUNICIPALITY
         AND C.NPROVINCE = AD.NPROVINCE
         AND D.NLOCAL = AD.NLOCAL
         AND TRIM(AD.SKEYADDRESS) || TO_CHAR(AD.DCOMPDATE, 'YYYYMMDDHH24MMSS') || TO_CHAR(AD.DEFFECDATE, 'YYYYMMDDHH24MMSS') =
             (SELECT /*+INDEX (ADT XIF2110ADDRESS)*/
               MAX(TRIM(AT.SKEYADDRESS) || TO_CHAR(AT.DCOMPDATE, 'YYYYMMDDHH24MMSS') || TO_CHAR(AT.DEFFECDATE, 'YYYYMMDDHH24MMSS'))
                FROM ADDRESS AT
               WHERE AT.SCLIENT = AD.SCLIENT
                 AND AT.NRECOWNER = 2
                 AND AT.SRECTYPE = 2
                 AND AT.DNULLDATE IS NULL);
    EXCEPTION
      WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE(SQLERRM || CHR(13) || DBMS_UTILITY.FORMAT_ERROR_BACKTRACE || '  ---1');
        NULL;
    END;

    IF V_SCLIENT_AD IS NOT NULL THEN
      
         
    V_SDIRECCION_ORD := INSUDB.PKG_REPORTES_TABLERO_CONTROL.
    FN_DIRE_CORREO_CLIENTE(P_SCLIENT => V_SCLIENT_ORD,
                           P_NRECOWNER => 2,
                           P_STIPO_DATO => 'D',
                           P_SIND_INDIVIDUAL => 'N');
    
    
      /*PKG_BDU_CLIENTE.SP_FORMA_FORMATDIRE(V_STI_DIRE,
                                          V_SNOM_DIRECCION,
                                          V_SNUM_DIRECCION,
                                          V_STI_BLOCKCHALET,
                                          V_SBLOCKCHALET,
                                          V_STI_INTERIOR,
                                          V_SNUM_INTERIOR,
                                          V_STI_CJHT,
                                          V_SNOM_CJHT,
                                          V_SETAPA,
                                          V_SMANZANA,
                                          V_SLOTE,
                                          V_SREFERENCIA,
                                          V_SDIRECCION_ORD);*/

    END IF;

    PKG_BDU_CLIENTE.SP_HOMOLDATOSOTROS('FIDELIZACION', 'RUBIGEO', V_NMUNICIPALITY_ORD, V_NMUNICIPALITY_ORD);

    --V_NPROVINCE_ORD := TRIM(TO_CHAR(V_NMUNICIPALITY_ORD, '000000'));
    --V_NLOCAL_ORD    := TRIM(TO_CHAR(V_NMUNICIPALITY_ORD, '000000'));

    BEGIN
    
    
      SELECT SPHONE
        INTO V_SPHONE_ORD
        FROM PHONES PH
       WHERE SUBSTR(PH.SKEYADDRESS, 2, 14) = V_SCLIENT_ORD
         AND PH.NRECOWNER = 2
         AND PH.DNULLDATE IS NULL
         AND TRIM(PH.SKEYADDRESS) || PH.NKEYPHONES || TO_CHAR(PH.DCOMPDATE, 'YYYYMMDDHH24MMSS') || TO_CHAR(PH.DEFFECDATE, 'YYYYMMDDHH24MMSS') =
             (SELECT /*+INDEX (PHT IDX_PHONE_1)*/
               MAX(TRIM(PHT.SKEYADDRESS) || PHT.NKEYPHONES || TO_CHAR(PHT.DCOMPDATE, 'YYYYMMDDHH24MMSS') || TO_CHAR(PHT.DEFFECDATE, 'YYYYMMDDHH24MMSS'))
                FROM PHONES PHT
               WHERE SUBSTR(PHT.SKEYADDRESS, 2, 14) = SUBSTR(PH.SKEYADDRESS, 2, 14)
                 AND PHT.NRECOWNER = 2
                 AND PHT.DNULLDATE IS NULL);

    EXCEPTION
      WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE(SQLERRM || CHR(13) || DBMS_UTILITY.FORMAT_ERROR_BACKTRACE || '  ---2');
        NULL;
    END;

    BEGIN
      OPEN C_TABLE FOR

        SELECT

        --TRIM(TO_CHAR(ROWNUM, '00000000')) AS FILA, --1-vb.801
         TRIM(TO_CHAR(ROWNUM, '00000000')) AS FILA, --1-vb.801
         '001 ' AS OFICINA, --2
         --TRIM(TO_CHAR(ROWNUM, '00000000')) AS OPERACION, --3-vb.802
         TRIM(TO_CHAR(ROWNUM, '00000000')) AS OPERACION, --3-vb.802
         RPAD(DECODE(TRIM(K.NCLAIM), '', ' ', TRIM(K.NCLAIM)), 20) AS INTERNO, --4
         K.MODALIDAD AS MODALIDAD, --5-vb.827
         '150141' AS OPE_UBIGEO, --6
         RPAD(DECODE(K.DLEDGER_DAT, '', ' ', TRIM(TO_CHAR(K.DLEDGER_DAT, 'YYYYMMDD'))), 8) AS OPE_FECHA, --7
         RPAD(DECODE(K.HORA, '', ' ', K.HORA), 6) AS OPE_HORA, --8
         RPAD(' ', 1) AS EJE_RELACION, --vb.835
         RPAD(' ', 1) AS EJE_CONDICION, --vb.836
         RPAD(' ', 1) AS EJE_TIPPER, --vb
         RPAD(' ', 1) AS EJE_TIPDOC, --vb
         RPAD(' ', 12) AS EJE_NUMDOC, --vb
         RPAD(' ', 11) AS EJE_NUMRUC, --vb
         RPAD(' ', 40) AS EJE_APEPAT, --vb
         RPAD(' ', 40) AS EJE_APEMAT, --vb
         RPAD(' ', 40) AS EJE_NOMBRES, --vb
         RPAD(' ', 4) AS EJE_OCUPACION, --vb
         --'' AS EJE_CIIU, --vb.845
         RPAD(' ', 6) AS EJE_PAIS,
         --'' AS EJE_DESCIIU, --vb.846
         RPAD(' ', 104) AS EJE_CARGO, --vb.847
         RPAD(' ', 2) AS EJE_PEP,
         RPAD(' ', 150) AS EJE_DOMICILIO, --vb.848
         RPAD(' ', 2) AS EJE_DEPART, --vb.849
         RPAD(' ', 2) AS EJE_PROV, --vb.850
         RPAD(' ', 2) AS EJE_DIST, --vb.851
         RPAD(' ', 40) AS EJE_TELEFONO, --vb.852

         RPAD(DECODE((SELECT MAX(NROLE) FROM ROLES RO
               WHERE RO.SCLIENT = V_SCLIENT_ORD),1,1,2), 1) AS ORD_RELACION, --27-vb.735
         RPAD(NVL(V_ORD_CONDICION, ' '), 1) AS ORD_CONDICION, --28-vb.736
         RPAD(NVL(V_ORD_TIPPER, ' '), 1) AS ORD_TIPPER, --29-vb.737
         --RPAD(DECODE(V_ORD_TIPDOC,'',' ',V_ORD_TIPDOC), 1) AS ORD_TIPDOC,
         RPAD(CASE
                WHEN V_ORD_TIPPER IN ('1', '2') THEN
                 NVL(V_ORD_TIPDOC, ' ')
                ELSE
                 ' '
              END

             ,
              1) AS ORD_TIPDOC, --30-vb.738
         RPAD(CASE
                WHEN V_ORD_TIPPER IN ('1', '2') THEN
                 NVL(V_SIDDOC_ORD, ' ')
                ELSE
                 ' '
              END,
              12) AS ORD_NUMDOC, --31-vb.671
         RPAD(CASE
                WHEN V_ORD_TIPPER IN (3, 4) THEN
                 NVL(V_SIDDOC_ORD, ' ')
                ELSE
                 ' '
              END,
              11) AS ORD_NUMRUC,
         --RPAD(DECODE(DECODE(RIGHT(V_SIDDOC_ORD, 11), '', ' ', RIGHT(V_SIDDOC_ORD, 11)), '', ' '), 11) AS ORD_NUMRUC, --32-vb.672
         RPAD(NVL(V_SCLIENAME_ORD, ' '), 120) AS ORD_APEPAT, --33
         RPAD(' ', 40) AS ORD_APEMAT, --34
         RPAD(' ', 40) AS ORD_NOMBRES, --35
         RPAD(' ', 4) AS ORD_OCUPACION, --36-vb.744
         RPAD(DECODE(V_ORD_PAIS, '', ' ', V_ORD_PAIS), 6) AS ORD_PAIS, --37
         --RPAD('' AS Ord_CIIU,--37-YA NO SE SOLICITA
         --RPAD('' AS Ord_DesCIIU,--38-YA NO SE SOLICITA
         RPAD(' ', 104) AS ORD_CARGO, --38-NO ES OBLIGATORIO.vb747
         RPAD(' ', 2) AS ORD_PEP, --39-NO ES OBLIGATORIO
         RPAD(UPPER(DECODE(V_SDIRECCION_ORD, '', ' ', TRIM(V_SDIRECCION_ORD))),150) AS ORD_DOMICILIO, --40
         RPAD(DECODE(V_NMUNICIPALITY_ORD, '', ' ', SUBSTR(V_NMUNICIPALITY_ORD, 1, 2)), 2) AS ORD_DEPART, --41
         RPAD(DECODE(V_NMUNICIPALITY_ORD, '', ' ', SUBSTR(V_NMUNICIPALITY_ORD, 3, 2)), 2) AS ORD_PROV, --42
         RPAD(DECODE(V_NMUNICIPALITY_ORD, '', ' ', SUBSTR(V_NMUNICIPALITY_ORD, 5, 2)), 2) AS ORD_DIST, --43
         --V_NMUNICIPALITY_ORD AS ORD_UBIGEO,
         
         RPAD(DECODE(INSTR(V_SPHONE_ORD,'.'),0,NVL(TRIM(V_SPHONE_ORD||''), ' '),' '), 40) AS ORD_TELEFONO, --44

         RPAD(DECODE((SELECT MAX(NROLE) FROM ROLES RO
               WHERE RO.SCLIENT = CLBENE.SCLIENT),1,1,2), 1) AS BEN_RELACION, --45
         RPAD('1', 1) AS BEN_CONDICION, --46
         RPAD(NVL(CLBENE.TIPO_PERSONA_BEN, ' '), 1) AS BEN_TIP_PER,
         --RPAD(DECODE(DECODE(SUBSTR(CLBENE.SCLIENT, 1, 2), '02', DECODE(CLBENE.STAX_CODE, '', '2', '1'), '01', '3', ' '), '', ' '), 1) AS BEN_TIP_PER, --47
         --RPAD(DECODE(DECODE(SUBSTR(CLBENE.SCLIENT, 1, 2), '02', DECODE(CLBENE.STAX_CODE, '', '2', '1'), '01', '3', ' '), '', ' '), 1) AS BEN_TIP_PER, --47
         --         RPAD(DECODE(DECODE(SUBSTR(CLBENE.SCLIENT, 1, 2), '02', '1', '01', ' ', ' '), '', ' '), 1) AS BEN_TIP_DOC, --48
         RPAD(NVL(CLBENE.TIPO_DOCUMENTO_BEN, ' '), 1) AS BEN_TIP_DOC,
         --RPAD(DECODE(DECODE(SUBSTR(CLBENE.SCLIENT, 1, 2), '02', RIGHT(CLBENE.SCLIENT, 8), '01', ' ', RIGHT(CLBENE.SCLIENT, 12)), '', ' '), 12) AS BEN_NUM_DOC, --49
         RPAD(CASE
                WHEN CLBENE.TIPO_PERSONA_BEN IN ('1', '2') THEN
                 NVL(CLBENE.NUM_DOC_BEN, ' ')
                ELSE
                 ' '
              END,
              12) AS BEN_NUM_DOC, --49
         RPAD(CASE
                WHEN CLBENE.TIPO_PERSONA_BEN IN ('3', '4') THEN
                 RIGHT(CLBENE.SCLIENT, 11)
                ELSE
                 ' '
              END,
              11) AS BEN_NUM_RUC,
         --RPAD(DECODE(DECODE(SUBSTR(CLBENE.SCLIENT, 1, 2), '02', CLBENE.STAX_CODE, '01', RIGHT(CLBENE.SCLIENT, 11), ' '), '', ' '), 11) AS BEN_NUM_RUC, --50
         RPAD(REPLACE(REPLACE(REPLACE(CASE
                                        WHEN CLBENE.TIPO_PERSONA_BEN IN ('1', '2') THEN
                                         NVL(CLBENE.SLASTNAME, ' ')
                                        WHEN CLBENE.TIPO_PERSONA_BEN IN ('3', '4') THEN
                                         NVL(CLBENE.SCLIENAME, ' ')
                                        ELSE
                                         ' '
                                      END,
                                      '?',
                                      '#'),
                              'Ñ',
                              '#'),
                      'ñ',
                      '#'),
              120) AS BEN_APEPAT, --51
         --RPAD(DECODE(REPLACE(REPLACE(REPLACE(DECODE(SUBSTR(CLBENE.SCLIENT, 1, 2), '02', CLBENE.SLASTNAME, '01', CLBENE.SCLIENAME, ' '), '?', '#'), 'Ñ', '#'), 'ñ', '#'), '', ' '), 120) AS BEN_APEPAT, --51
         RPAD(REPLACE(REPLACE(REPLACE(NVL(CLBENE.SLASTNAME2, ' '), '?', '#'), 'Ñ', '#'), 'ñ', '#'), 40) AS BEN_APEMAT, --52
         RPAD(REPLACE(REPLACE(REPLACE(NVL(CLBENE.SFIRSTNAME, ' '), '?', '#'), 'Ñ', '#'), 'ñ', '#'), 40) AS BEN_NOMBRES, --53

         RPAD(NVL(CLBENE.COD_ESPECIALIDAD_SBS_BEN, '0999'), 4) AS BEN_OCUPACION, --54
         --RPAD('' AS Ben_CIIU,--55-YA NO SE SOLICITA
         RPAD('PE', 6) /*tb66.sdescript*/ AS BEN_PAIS, --55
         --RPAD('' AS Ben_Des_CIIU,--56-YA NO SE SOLICITA
         RPAD(' ', 104) AS BEN_CARGO, --56-NO ES OBLIGATORIO
         RPAD(' ', 2) AS BEN_PEP, --57-NO ES OBLIGATORIO
         --CLBENE.SSTREET AS DIRECCION_B,
         RPAD(REPLACE(REPLACE(REPLACE(UPPER(DECODE(CLBENE.SSTREET, '', ' ', TRIM(CLBENE.SSTREET)))
                                     ,'?','#'),'Ñ','#'),'ñ','#'),150) AS BEN_DOMICILIO, --58
         --TO_NUMBER(CLBENE.COD_UBI_CLI) AS BEN_UBIGEO,
         RPAD(DECODE(CLBENE.NMUNICIPALITY, '', ' ', SUBSTR(TRIM(TO_CHAR((CLBENE.COD_UBI_CLI), '000000')), 1, 2)), 2) AS BEN_DEPART, --59
         RPAD(DECODE(CLBENE.NMUNICIPALITY, '', ' ', SUBSTR(TRIM(TO_CHAR((CLBENE.COD_UBI_CLI), '000000')), 3, 2)), 2) AS BEN_PROV, --60
         RPAD(DECODE(CLBENE.NMUNICIPALITY, '', ' ', SUBSTR(TRIM(TO_CHAR((CLBENE.COD_UBI_CLI), '000000')), 5, 2)), 2) AS BEN_DIST, --61
         RPAD(DECODE(INSTR(CLBENE.SPHONE,'.'),0,NVL(TRIM(CLBENE.SPHONE||''), ' '),' '), 40) AS BEN_TELEFONO, --62
         RPAD(DECODE(V_DAT_TIPFON, '', ' ', V_DAT_TIPFON), 1) AS DAT_TIPFON, --63-vb.807
         RPAD(DECODE(V_DAT_TIPOPE, '', ' ', V_DAT_TIPOPE), 2) AS DAT_TIPOPE, --64-vb.808
         RPAD(' ', 40) AS DAT_DESOPE, --65-vb.809
         RPAD(' ', 80) AS DAT_ORIFON, --66-vb.810
         RPAD(DECODE(K.NCURRENCYPAY, '1', 'PEN', 'USD'), 3) AS DAT_MONOPE, --67
         RPAD(' ', 3) AS DAT_MONOPE_A, --68
         RPAD(TRIM(TO_CHAR(CAST((K.NAMOUNT) AS NUMBER(15, 2)), '000000000000000.00')), 29) AS DAT_MTOOPE, --MTO_PENSIONGAR , MTO_PRIUNI 69
         RPAD(' ', 30) AS DAT_MTOOPEA, --MTO_PENSIONGAR , MTO_PRIUNI 70
         RPAD(' ', 5) AS DAT_COD_ENT_INVO, --71--NO OBLIGATORIO
         RPAD(' ', 1) AS DAT_COD_TIP_CTAO, --72--NO OBLIGATORIO
         RPAD(' ', 20) AS DAT_COD_CTAO, --73--NO OBLIGATORIO
         RPAD(' ', 150) AS DAT_ENT_FNC_EXTO, --74--NO OBLIGATORIO
         RPAD(' ', 5) AS DAT_COD_ENT_INVB, --75--NO OBLIGATORIO
         RPAD(' ', 1) AS DAT_COD_TIP_CTAB, --76--NO OBLIGATORIO
         RPAD(' ', 20) AS DAT_COD_CTAB, --77--NO OBLIGATORIO
         RPAD(' ', 150) AS DAT_ENT_FNC_EXTB, --78--NO OBLIGATORIO
         RPAD(DECODE(V_DAT_ALCANCE, '', ' ', V_DAT_ALCANCE), 1) AS DAT_ALCANCE, --79-vb.883
         RPAD(' ', 2) AS DAT_COD_PAISO, --80--ALCANCE 1, ESTE CAMPO VA EN BLANCO
         RPAD(' ', 2) AS DAT_COD_PAISD, --81--ALCANCE 1, ESTE CAMPO VA EN BLANCO
         RPAD('2', 1) AS DAT_INTOPE, --82--NO OBLIGATORIO
         RPAD(DECODE(K.DAT_FORMA, '', ' ', K.DAT_FORMA), 1) AS DAT_FORMA, --83-vb.887
         RPAD(DECODE(K.TIPOPAGO, '', ' ', K.TIPOPAGO), 40) AS DAT_INFORM, --84--NO OBLIGATORIO
         /*RPAD((SELECT DISTINCT DECODE(NIDPAIDTYPE, 1, 'Otros', 2, 'Medios o plataformas virtual', 3, 'Medios o plataformas virtual',' ')
             FROM PV_PAYROLL_PAYMENT PPP,
                  PV_PAYROLL_DETAIL  PPD
            WHERE PPD.NIDPAYROLL = PPP.NIDPAYROLL
              AND PPD.NRECEIPT = K.NRECEIPT), 40) AS DAT_INFORM,*/
         'SIN' AS ORIGEN

          FROM (
               SELECT   MODALIDAD,
                        BENEFICIARIO,
                        NPOLICY,
                        NPRODUCT,
                        SDESCRIPT,
                        DLEDGER_DAT,
                        NCERTIF,
                        NCLAIM,
                        NAMOUNT,
                        NCURRENCYPAY,
                        TIPO_CAMBIO,
                        SOLES,
                        DOLARES,
                        --B.DCOMPDATE,
                        HORA,
                        ASEGURADO,
                        --NVL(C.SDESCRIPT, '') AS TIPOPAGO,
                        TIPOPAGO,
                        NRECEIPT,
                        DAT_FORMA
                FROM (SELECT DISTINCT 'U' AS MODALIDAD,
                        B.SCLIENT AS BENEFICIARIO,
                        A.NPOLICY,
                        A.NPRODUCT,
                        D.SDESCRIPT,
                        B.DLEDGER_DAT,
                        A.NCERTIF,
                        A.NCLAIM,
                        E.NAMOUNT,
                        B.NCURRENCYPAY,
                        P_TC AS TIPO_CAMBIO,
                        TO_CHAR(E.SOLES) AS SOLES,
                        TO_CHAR(E.DOLARES) AS DOLARES,
                        
                        --B.DCOMPDATE,
                        '200000' AS HORA,
                        (SELECT DISTINCT SCLIENT
                           FROM CL_COVER
                          WHERE NCLAIM = A.NCLAIM) AS ASEGURADO,
                        --NVL(C.SDESCRIPT, '') AS TIPOPAGO,
                        DECODE(B.SREQUEST_TY,4,'Medios o plataformas virtual',' ') AS TIPOPAGO,
                        B.NRECEIPT,
                        DECODE(B.SREQUEST_TY,4,'3',' ') AS DAT_FORMA
                  FROM CLAIM A
                 INNER JOIN CHEQUES B
                    ON A.NCLAIM = B.NCLAIM
                 INNER JOIN PRODMASTER D
                    ON A.NPRODUCT = D.NPRODUCT
                    AND B.NBRANCH = D.NBRANCH
                  /*LEFT JOIN TABLE193 C
                    ON C.SREQUEST_TY = B.SREQUEST_TY*/
                 INNER JOIN (SELECT *
                               FROM (SELECT A.NPOLICY,
                                            A.NPRODUCT,
                                            A.NCERTIF,
                                            A.NCLAIM,
                                            SUM(B.NAMOUNT) AS NAMOUNT,
                                            B.NCURRENCYPAY,
                                            TO_CHAR(SUM(DECODE(B.NCURRENCYPAY, 1, B.NAMOUNT, 0))) AS SOLES,
                                            TO_CHAR(SUM(DECODE(B.NCURRENCYPAY, 2, B.NAMOUNT, ROUND(B.NAMOUNT / P_TC, 4)))) AS DOLARES,
                                            (SELECT DISTINCT SCLIENT
                                               FROM CL_COVER
                                              WHERE NCLAIM = A.NCLAIM 
                                              AND SCLIENT = A.SCLIENT) AS ASEGURADO
                                       FROM CLAIM A
                                      INNER JOIN CHEQUES B
                                         ON A.NCLAIM = B.NCLAIM
                                      WHERE NOT B.NCLAIM IS NULL
                                        AND TRUNC(B.DLEDGER_DAT) BETWEEN P_FECINI AND P_FECFIN
                                      GROUP BY A.NPOLICY,
                                               A.NPRODUCT,
                                               A.NCERTIF,
                                               A.NCLAIM,
                                               B.NCURRENCYPAY)
                              WHERE DOLARES > P_MONTO) E
                    ON E.NPOLICY = A.NPOLICY
                   AND E.NPRODUCT = A.NPRODUCT
                   AND E.NCERTIF = A.NCERTIF
                   AND E.NCLAIM = A.NCLAIM
                 WHERE (P_OPE = 1 OR P_OPE = 3)
                   AND TRUNC(B.DLEDGER_DAT) BETWEEN P_FECINI AND P_FECFIN

                UNION ALL

                SELECT *
                  FROM (SELECT DISTINCT 'M' AS MODALIDAD,
                                B.SCLIENT AS BENEFICIARIO,
                                A.NPOLICY,
                                A.NPRODUCT,
                                D.SDESCRIPT,
                                B.DLEDGER_DAT,
                                A.NCERTIF,
                                A.NCLAIM,
                                B.NAMOUNT,
                                B.NCURRENCYPAY,
                                P_TC AS TIPO_CAMBIO,
                                TO_CHAR(DECODE(B.NCURRENCYPAY, 1, B.NAMOUNT, 0)) AS SOLES,
                                TO_CHAR(DECODE(B.NCURRENCYPAY, 2, B.NAMOUNT, ROUND(B.NAMOUNT / P_TC, 4))) AS DOLARES,
                                --B.DCOMPDATE,
                                '200000' AS HORA,
                                (SELECT DISTINCT SCLIENT
                                   FROM CL_COVER
                                  WHERE NCLAIM = A.NCLAIM) AS ASEGURADO,
                                --NVL(C.SDESCRIPT, '') AS TIPOPAGO,
                                DECODE(B.SREQUEST_TY,4,'Medios o plataformas virtual',' ') AS TIPOPAGO,
                                B.NRECEIPT,
                                DECODE(B.SREQUEST_TY,4,'3',' ') AS DAT_FORMA
                           FROM CLAIM A
                          INNER JOIN CHEQUES B
                             ON A.NCLAIM = B.NCLAIM
                          INNER JOIN PRODMASTER D
                             ON A.NPRODUCT = D.NPRODUCT
                             AND B.NBRANCH = D.NBRANCH
                           /*LEFT JOIN TABLE193 C
                             ON C.SREQUEST_TY = B.SREQUEST_TY*/
                          INNER JOIN CLIENT CL
                             ON CL.SCLIENT = B.SCLIENT
                          WHERE TRUNC(B.DLEDGER_DAT) BETWEEN P_FECINI AND P_FECFIN
                            AND NOT B.NCLAIM IS NULL) Z
                 WHERE Z.BENEFICIARIO IN ((SELECT X.SCLIENT
                                            FROM (SELECT A.NPOLICY,
                                                         A.NPRODUCT,
                                                         D.SDESCRIPT,
                                                         B.DLEDGER_DAT,
                                                         A.NCERTIF,
                                                         A.NCLAIM,
                                                         B.SCLIENT,
                                                         B.NAMOUNT,
                                                         B.NCURRENCYPAY,
                                                         P_TC AS TIPO_CAMBIO,
                                                         TO_CHAR(DECODE(B.NCURRENCYPAY, 1, B.NAMOUNT, ROUND(B.NAMOUNT / P_TC, 4))) AS SOLES,
                                                         TO_CHAR(DECODE(B.NCURRENCYPAY, 2, B.NAMOUNT, ROUND(B.NAMOUNT / P_TC, 4))) AS DOLARES
                                                    FROM CLAIM A
                                                   INNER JOIN CHEQUES B
                                                      ON A.NCLAIM = B.NCLAIM
                                                   INNER JOIN PRODMASTER D
                                                      ON A.NPRODUCT = D.NPRODUCT
                                                   WHERE TRUNC(B.DLEDGER_DAT) BETWEEN P_FECINI AND P_FECFIN
                                                     AND NOT B.NCLAIM IS NULL) X
                                           GROUP BY X.SCLIENT
                                          HAVING SUM(X.DOLARES) > P_MONTO AND COUNT(*) > 1))
                   AND (P_OPE = 2 OR P_OPE = 3)
                   AND Z.DOLARES >= 1000)
                --order by 1

                ) K

          LEFT OUTER JOIN (SELECT
                           /*+INDEX(EQUI SYS_C00167813) INDEX(c XPKPROVINCE) INDEX(d XPKTAB_LOCAT) INDEX(e XPKMUNICIPALITY) INDEX(cc CLIENT_COMPLEMENT_PK)*/
                            A.NPERSON_TYP,
                            A.SCLIENAME,
                            A.SFIRSTNAME,
                            A.SLASTNAME,
                            A.SLASTNAME2,
                            A.NQ_CHILD,
                            INSUDB.PKG_REPORTES_TABLERO_CONTROL.
                            FN_DIRE_CORREO_CLIENTE(P_SCLIENT => A.SCLIENT,
                                                   P_NRECOWNER => 2,
                                                   P_STIPO_DATO => 'D',
                                                   P_SIND_INDIVIDUAL => 'N') AS SSTREET,
                            C.SDESCRIPT AS DEPART,
                            D.SDESCRIPT AS PROV,
                            E.SDESCRIPT AS DISTRITO,
                            PH.SPHONE,
                            A.STAX_CODE,
                            E.NMUNICIPALITY,
                            A.SCLIENT,
                            EQUI.COD_UBI_CLI,
                            DCLI.NTYPCLIENTDOC,
                            DCLI.SCLINUMDOCU,
                            DCLI.SCLINUMDOCU AS NUM_DOC_BEN,
                            A.NPERSON_TYP AS TIP_PER,
                            A.NNATIONALITY,
                            NI.SCOD_COUNTRY_ISO,
                            CASE
                              WHEN SUBSTR(A.SCLIENT, 1, 2) = '01' THEN
                               CASE
                                 WHEN SUBSTR(A.SCLIENT, 4, 2) = '20' THEN
                                  '3' --'Persona juridica'
                                 ELSE
                                  '1' --'persona natural con negocio'
                               END
                              ELSE
                               '2' --'Persona natural'
                            END AS TIPO_PERSONA_BEN,
                            CASE
                              WHEN DCLI.NTYPCLIENTDOC = 2 THEN
                               '1'
                              WHEN DCLI.NTYPCLIENTDOC = 4 THEN
                               '2'
                              WHEN DCLI.NTYPCLIENTDOC = 6 THEN
                               '5'
                              WHEN DCLI.NTYPCLIENTDOC = 1 THEN
                               ' '
                              WHEN DCLI.NTYPCLIENTDOC = 3 OR DCLI.NTYPCLIENTDOC = 5 OR DCLI.NTYPCLIENTDOC = 7 OR DCLI.NTYPCLIENTDOC = 8 OR DCLI.NTYPCLIENTDOC = 9 OR DCLI.NTYPCLIENTDOC = 10 OR DCLI.NTYPCLIENTDOC = 11 OR
                                   DCLI.NTYPCLIENTDOC = 0 OR DCLI.NTYPCLIENTDOC = 12 OR DCLI.NTYPCLIENTDOC = 13 THEN
                               '9'
                              ELSE
                               ' '
                            END AS TIPO_DOCUMENTO_BEN,
                            COS.NIDOCUP_SBS AS COD_ESPECIALIDAD_SBS_BEN
                             FROM CLIENT A

                             LEFT OUTER JOIN (SELECT SCLIENT,
                                                    NLOCAL,
                                                    NPROVINCE,
                                                    NMUNICIPALITY,
                                                    NCOUNTRY,
                                                    SKEYADDRESS,
                                                    SSTREET
                                               FROM ADDRESS ADRR
                                              WHERE ADRR.NRECOWNER = 2
                                                AND ADRR.SRECTYPE = 2
                                                AND ADRR.DNULLDATE IS NULL
                                                AND TRIM(ADRR.SKEYADDRESS) || TO_CHAR(ADRR.DCOMPDATE, 'YYYYMMDDHH24MMSS') || TO_CHAR(ADRR.DEFFECDATE, 'YYYYMMDDHH24MMSS') =
                                                    (SELECT /*+INDEX (AT XIF2110ADDRESS)*/
                                                      MAX(TRIM(AT.SKEYADDRESS) || TO_CHAR(AT.DCOMPDATE, 'YYYYMMDDHH24MMSS') || TO_CHAR(AT.DEFFECDATE, 'YYYYMMDDHH24MMSS'))
                                                       FROM ADDRESS AT
                                                      WHERE AT.SCLIENT = ADRR.SCLIENT
                                                        AND AT.NRECOWNER = 2
                                                        AND AT.SRECTYPE = 2
                                                        AND AT.DNULLDATE IS NULL)

                                             ) ADR
                               ON ADR.SCLIENT = A.SCLIENT

                             LEFT OUTER JOIN (SELECT
                                             /*+INDEX (PHR IDX_PHONE_1)*/
                                              SPHONE,
                                              NKEYPHONES,
                                              SKEYADDRESS,
                                              DCOMPDATE,
                                              DEFFECDATE
                                               FROM PHONES PHR
                                              WHERE PHR.NRECOWNER = 2
                                                AND PHR.DNULLDATE IS NULL
                                                AND TRIM(PHR.SKEYADDRESS) || PHR.NKEYPHONES || TO_CHAR(PHR.DCOMPDATE, 'YYYYMMDDHH24MMSS') || TO_CHAR(PHR.DEFFECDATE, 'YYYYMMDDHH24MMSS') =
                                                    (SELECT /*+INDEX (PHT IDX_PHONE_1)*/
                                                      MAX(TRIM(PHT.SKEYADDRESS) || PHT.NKEYPHONES || TO_CHAR(PHT.DCOMPDATE, 'YYYYMMDDHH24MMSS') || TO_CHAR(PHT.DEFFECDATE, 'YYYYMMDDHH24MMSS'))
                                                       FROM PHONES PHT
                                                      WHERE SUBSTR(PHT.SKEYADDRESS, 2, 14) = SUBSTR(PHR.SKEYADDRESS, 2, 14)
                                                        AND PHT.NRECOWNER = 2
                                                        AND PHT.DNULLDATE IS NULL)

                                             ) PH
                           --ON PH.SKEYADDRESS = ADR.SKEYADDRESS
                               ON SUBSTR(PH.SKEYADDRESS, 2, 14) = A.SCLIENT

                             LEFT JOIN PROVINCE C
                               ON C.NPROVINCE = ADR.NPROVINCE
                             LEFT JOIN TAB_LOCAT D
                               ON D.NLOCAL = ADR.NLOCAL
                             LEFT JOIN MUNICIPALITY E
                               ON E.NMUNICIPALITY = ADR.NMUNICIPALITY
                             LEFT OUTER JOIN EQUI_UBIGEO EQUI
                               ON TO_NUMBER(EQUI.COD_UBI_DIS) = E.NMUNICIPALITY
                              AND EQUI.COD_CLI = '11111111111111'
                             LEFT OUTER JOIN CLIDOCUMENTS DCLI
                               ON DCLI.SCLIENT = A.SCLIENT
                             LEFT OUTER JOIN TBL_TRX_NATIONALITY_ISO NI
                               ON NVL(A.NNATIONALITY, ADR.NCOUNTRY) = NI.NNATIONALITY
                              AND NI.SACTIVE = 1
                             LEFT OUTER JOIN TBL_CONFIG_OCUP_SBS COS
                               ON COS.NIDOCUPACION = A.NSPECIALITY
                              AND COS.SORIGEN_BD = 'TIME'
                              AND COS.SACTIVE = '1') CLBENE
            ON CLBENE.SCLIENT = K.BENEFICIARIO;

    EXCEPTION
      WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE(SQLERRM || CHR(13) || DBMS_UTILITY.FORMAT_ERROR_BACKTRACE || '  ---2');

    END;

  END SINIESTROS_MAYORES;

  PROCEDURE RENTAS_MAYORES(P_FECINI      VARCHAR2,
                           P_FECFIN      VARCHAR2,
                           P_MONTO_UNICO NUMBER,
                           P_TC          NUMBER,
                           C_TABLE       OUT MYCURSOR) IS

    P_FEC_INI VARCHAR2(20);
    P_FEC_FIN VARCHAR2(20);

  BEGIN
    P_FEC_INI := TRIM(TO_CHAR(TO_DATE(P_FECINI, 'DD/MM/YYYY'), 'YYYYMMDD'));
    P_FEC_FIN := TRIM(TO_CHAR(TO_DATE(P_FECFIN, 'DD/MM/YYYY'), 'YYYYMMDD'));
    OPEN C_TABLE FOR

      SELECT TRIM(TO_CHAR(ROWNUM, '00000000')) AS FILA,
             TRIM(TO_CHAR(ROWNUM, '00000000')) AS OPERACION,
             Z.*
        FROM (SELECT
              --SUBSTR(a.FEC_TRASPASO,1,6) AS sPeriodo,--'sPeriodo',
              --TO_CHAR(ROWNUM, '00000000') AS FILA,
               '001 ' AS OFICINA,
               --TO_CHAR(ROWNUM, '00000000') AS OPERACION,
               RPAD(P.PRODUCTO || TRIM(P.NUM_POLIZA), 20) AS INTERNO,
               'U' AS MODALIDAD, --vb.832
               '150141' AS OPE_UBIGEO, --vb.834
               RPAD(DECODE(P.FEC_TRASPASO, '', ' ', P.FEC_TRASPASO), 8) AS OPE_FECHA,
               RPAD(DECODE(P.HOR_CREA, '', ' ', P.HOR_CREA), 6) AS OPE_HORA,
               RPAD(' ', 1) AS EJE_RELACION, --vb.835
               RPAD(' ', 1) AS EJE_CONDICION, --vb.836
               RPAD(' ', 1) AS EJE_TIPPER, --vb.837
               RPAD(' ', 1) AS EJE_TIPDOC, --vb.838
               RPAD(' ', 12) AS EJE_NUMDOC, --vb.839
               RPAD(' ', 11) AS EJE_NUMRUC, --vb.840
               RPAD(' ', 40) AS EJE_APEPAT, --vb.841
               RPAD(' ', 40) AS EJE_APEMAT, --vb.842
               RPAD(' ', 40) AS EJE_NOMBRES, --vb.843
               RPAD(' ', 4) AS EJE_OCUPACION, --vb.844
               --'' AS EJE_CIIU, --vb.845
               RPAD(' ', 6) AS EJE_PAIS,
               --'' AS EJE_DESCIIU, --vb.846
               RPAD(' ', 104) AS EJE_CARGO, --vb.847
               RPAD(' ', 2) AS EJE_PEP,
               RPAD(' ', 150) AS EJE_DOMICILIO, --vb.848
               RPAD(' ', 2) AS EJE_DEPART, --vb.849
               RPAD(' ', 2) AS EJE_PROV, --vb.850
               RPAD(' ', 2) AS EJE_DIST, --vb.851
               RPAD(' ', 40) AS EJE_TELEFONO, --vb.852

               RPAD(DECODE(NPERSON_TYP,1,1,2), 1) AS ORD_RELACION, --vb.673
               RPAD('1', 1) AS ORD_CONDICION, --vb.674
               RPAD('2', 1) AS ORD_TIPPER, --vb.675
               RPAD(DECODE(P.COD_TIPOIDENBEN, '1', '1', '2', '2', '5', '5', '9'), 1) /*'1'*/ AS ORD_TIPDOC,
               RPAD(DECODE(P.NUM_IDENBEN, '', ' ', P.NUM_IDENBEN), 12) AS ORD_NUMDOC,
               RPAD(' ', 11) /*''*/ AS ORD_NUMRUC,
               RPAD(REPLACE(REPLACE(REPLACE(P.GLS_PATBEN, '?', '#'), 'Ñ', '#'), 'ñ', '#'), 120) AS ORD_APEPAT, --vb.547
               RPAD(REPLACE(REPLACE(REPLACE(P.GLS_MATBEN, '?', '#'), 'Ñ', '#'), 'ñ', '#'), 40) AS ORD_APEMAT, --vb.547
               RPAD(REPLACE(REPLACE(REPLACE(P.GLS_NOMBEN, '?', '#'), 'Ñ', '#'), 'ñ', '#') || ' ' || NVL(REPLACE(REPLACE(REPLACE(P.GLS_NOMSEGBEN, '?', '#'), 'Ñ', '#'), 'ñ', '#'), ''), 40) AS ORD_NOMBRES,
               RPAD(NVL(P.COD_OCUPACION, '0999'), 4) AS ORD_OCUPACION,
               --'' AS ORD_CIIU,
               RPAD(NVL(NI.SCOD_COUNTRY_ISO, 'PE'), 6) AS ORD_PAIS,
               --F.DESC_OCUPACION AS ORD_DESCIIU,
               RPAD(DECODE(P.CARGO, '', ' ', P.CARGO), 104) AS ORD_CARGO,
               RPAD(' ', 2) AS ORD_PEP,
               RPAD(


               REPLACE(REPLACE(REPLACE(

               UPPER(DECODE(P.GLS_DIRBEN,'',' ',TRIM(P.GLS_DIRBEN))),'?', '#'),'Ñ','#'),'ñ','#')

                    ,150) AS ORD_DOMICILIO,
               --RPAD(NVL(DECODE(SUBSTR(LTRIM(EQUI.COD_UBI_CLI, '0'), 1, 2), '', '15'), ' '), 2) AS ORD_DEPART,
               --RPAD(NVL(DECODE(SUBSTR(LTRIM(EQUI.COD_UBI_CLI, '0'), 3, 2), '', '01'), ' '), 2) AS ORD_PROV,
               --RPAD(NVL(DECODE(SUBSTR(LTRIM(EQUI.COD_UBI_CLI, '0'), 5, 2), '', '41'), ' '), 2) AS ORD_DIST,
               RPAD(NVL(SUBSTR(LTRIM(EQUI.COD_UBI_CLI, '0'), 1, 2), ' '), 2) AS ORD_DEPART,
               RPAD(NVL(SUBSTR(LTRIM(EQUI.COD_UBI_CLI, '0'), 3, 2), ' '), 2) AS ORD_PROV,
               RPAD(NVL(SUBSTR(LTRIM(EQUI.COD_UBI_CLI, '0'), 5, 2), ' '), 2) AS ORD_DIST,
               RPAD(DECODE(INSTR(NVL(P.GLS_TELBEN2, NVL(P.GLS_FONOBEN, ' ')),'.'),0,NVL(TRIM(NVL(P.GLS_TELBEN2, NVL(P.GLS_FONOBEN, ' '))||''), ' ')), 40) AS ORD_TELEFONO,
               RPAD(DECODE(NPERSON_TYP,1,1,2), 1) AS BEN_RELACION, --vb.730
               RPAD('1', 1) AS BEN_CONDICION, --vb.731
               RPAD('2', 1) AS BEN_TIP_PER, --homologar

               RPAD(DECODE(P.COD_TIPOIDENBEN, '1', '1', '2', '2', '5', '5', '9'), 1) AS BEN_TIP_DOC,
               RPAD(DECODE(P.NUM_IDENBEN, '', ' ', P.NUM_IDENBEN), 12) BEN_NUM_DOC,
               RPAD(' ', 11) AS BEN_NUM_RUC,
               RPAD(REPLACE(REPLACE(REPLACE(P.GLS_PATBEN, '?', '#'), 'Ñ', '#'), 'ñ', '#'), 120) AS BEN_APEPAT,
               RPAD(REPLACE(REPLACE(REPLACE(P.GLS_MATBEN, '?', '#'), 'Ñ', '#'), 'ñ', '#'), 40) AS BEN_APEMAT,
               RPAD(REPLACE(REPLACE(REPLACE(P.GLS_NOMBEN, '?', '#'), 'Ñ', '#'), 'ñ', '#') || ' ' || NVL(REPLACE(REPLACE(REPLACE(P.GLS_NOMSEGBEN, '?', '#'), 'Ñ', '#'), 'ñ', '#'), ''), 40) AS BEN_NOMBRES,
               RPAD(NVL(P.COD_OCUPACION, '0999'), 4) AS BEN_OCUPACION,
               --'' AS BEN_CIIU,
               --RPAD(DECODE(DIR.COD_PAIS, 604, 'PE', ' '), 6) AS BEN_PAIS,
               RPAD(DECODE(NI.SCOD_COUNTRY_ISO, '', 'PE', NI.SCOD_COUNTRY_ISO), 6) AS BEN_PAIS,
               --'' AS BEN_DESCIIU, --F.DESC_OCUPACION AS BEN_DESCIIU,
               RPAD(DECODE(P.CARGO, '', ' ', P.CARGO), 104) AS BEN_CARGO,
               RPAD(' ', 2) AS BEN_PEP, --F.CARGO AS BEN_CARGO,
               RPAD(

               REPLACE(REPLACE(REPLACE(
               UPPER(
               DECODE(P.GLS_DIRBEN,'',' ',TRIM(P.GLS_DIRBEN)))
                    , '?', '#'),'Ñ','#'),
                    'ñ',
                    '#')
                    ,150) AS BEN_DOMICILIO,
               --C.COD_COMUNA AS UBIGEO,
               RPAD(NVL(SUBSTR(LTRIM(EQUI.COD_UBI_CLI, '0'), 1, 2), ' '), 2) AS BEN_DEPART,
               RPAD(NVL(SUBSTR(LTRIM(EQUI.COD_UBI_CLI, '0'), 3, 2), ' '), 2) AS BEN_PROV,
               RPAD(NVL(SUBSTR(LTRIM(EQUI.COD_UBI_CLI, '0'), 5, 2), ' '), 2) AS BEN_DIST,
               RPAD(DECODE(INSTR(NVL(P.GLS_TELBEN2, NVL(P.GLS_FONOBEN, ' ')),'.'),0,NVL(TRIM(NVL(P.GLS_TELBEN2, NVL(P.GLS_FONOBEN, ' '))||' '), ' ')), 40) AS BEN_TELEFONO,
               RPAD('2', 1) AS DAT_TIPFON, --vb.796
               RPAD('34', 2) AS DAT_TIPOPE, --vb.797
               RPAD(' ', 40) AS DAT_DESOPE, --vb.798
               RPAD(' ', 80) AS DAT_ORIFON, --vb.799
               RPAD(DECODE(P.MONEDA, 'NS', 'PEN', 'USD'), 3) AS DAT_MONOPE,
               RPAD(' ', 3) AS DAT_MONOPE_A, --vb.854
               RPAD(TO_CHAR(P.MONTO, '00000000000000000000000000.00'), 29) AS DAT_MTOOPE,
               RPAD(' ', 30) AS DAT_MTOOPEA, --70
               RPAD(' ', 5) AS DAT_COD_ENT_INVO, --71--NO OBLIGATORIO
               RPAD('1', 1) AS DAT_COD_TIP_CTAO, --72--vb.856-NO OBLIGATORIO
               RPAD(' ', 20) AS DAT_COD_CTAO, --73--NO OBLIGATORIO
               RPAD(' ', 150) AS DAT_ENT_FNC_EXTO, --74--vb.859-NO OBLIGATORIO
               RPAD(' ', 5) AS DAT_COD_ENT_INVB, --75--vb.1290-NO OBLIGATORIO
               RPAD(' ', 1) AS DAT_COD_TIP_CTAB, --76--vb.1292-NO OBLIGATORIO
               RPAD(' ', 20) AS DAT_COD_CTAB, --77--NO OBLIGATORIO
               RPAD(' ', 150) AS DAT_ENT_FNC_EXTB, --78-vb.864--NO OBLIGATORIO
               RPAD('1', 1) AS DAT_ALCANCE, --79-vb.865
               RPAD(' ', 2) AS DAT_COD_PAISO, --80-vb.866
               RPAD(' ', 2) AS DAT_COD_PAISD, --81-vb.867
               RPAD('2', 1) AS DAT_INTOPE, --82-vb.868
               --RPAD('2', 1) AS DAT_FORMA, --83-vb.869
               RPAD(NVL((SELECT CASE
                          WHEN INSTR(UPPER(NVL(P.INFORM, ' ')), 'ABO') > 0 THEN
                           '9'
                          WHEN INSTR(UPPER(NVL(P.INFORM, ' ')), 'TRANS') > 0 THEN
                           '3'
                          WHEN INSTR(UPPER(NVL(P.INFORM, ' ')), 'CHEQUE') > 0 THEN
                           '9'
                        END
                   FROM DUAL),' '), 1) AS DAT_FORMA,
               --RPAD(NVL(P.INFORM, ' '), 40) AS DAT_INFORM, --84
               RPAD(NVL((SELECT CASE
                          WHEN INSTR(UPPER(NVL(P.INFORM, ' ')), 'ABO') > 0 THEN
                           'Otros'
                          WHEN INSTR(UPPER(NVL(P.INFORM, ' ')), 'TRANS') > 0 THEN
                           'Medios o plataformas virtual'
                          WHEN INSTR(UPPER(NVL(P.INFORM, ' ')), 'CHEQUE') > 0 THEN
                           'Otros'
                        END
                   FROM DUAL),' '), 40) AS DAT_INFORM,
               P.PRODUCTO AS ORIGEN --TIPO,
              --'PRI' AS ORIGEN
                FROM (
                      /*--RV
                      SELECT A.NUM_POLIZA,
                              A.FEC_TRASPASO,
                              A.HOR_CREA,
                              B.COD_TIPOIDENBEN,
                              B.NUM_IDENBEN,
                              B.GLS_PATBEN,
                              B.GLS_MATBEN,
                              B.GLS_NOMBEN,
                              B.GLS_NOMSEGBEN,
                              B.GLS_DIRBEN,
                              B.GLS_TELBEN2,
                              B.GLS_FONOBEN,
                              F.COD_OCUPACION,
                              DIR.COD_PAIS,
                              C.GLS_COMUNA,
                              D.GLS_PROVINCIA,
                              E.GLS_REGION,

                              C.COD_COMUNA,
                              F.CARGO,
                              A.COD_MONEDAPRIINF AS MONEDA,
                              A.MTO_PRIINF AS MONTO,
                              'Abonos bancarios - Interbank' AS INFORM,
                              'RV' AS PRODUCTO
                        FROM PD_TMAE_POLPRIREC@DBL_RENTASV.PROTECTA.DOM A
                        LEFT JOIN PP_TMAE_BEN@DBL_RENTASV.PROTECTA.DOM B
                          ON A.NUM_POLIZA = B.NUM_POLIZA
                        LEFT JOIN MA_TPAR_COMUNA@DBL_RENTASV.PROTECTA.DOM C
                          ON C.COD_DIRECCION = B.COD_DIRECCION
                        LEFT JOIN MA_TPAR_PROVINCIA@DBL_RENTASV.PROTECTA.DOM D
                          ON D.COD_PROVINCIA = C.COD_PROVINCIA
                        LEFT JOIN MA_TPAR_REGION@DBL_RENTASV.PROTECTA.DOM E
                          ON E.COD_REGION = C.COD_REGION
                        LEFT JOIN PP_TMAE_BENAUX@DBL_RENTASV.PROTECTA.DOM F
                          ON B.NUM_POLIZA = F.NUM_POLIZA
                        LEFT JOIN PP_TMAE_BEN_DIRECCION@DBL_RENTASV.PROTECTA.DOM DIR
                          ON DIR.NUM_POLIZA = B.NUM_POLIZA
                         AND DIR.NUM_ENDOSO = B.NUM_ENDOSO
                         AND DIR.NUM_ORDEN = B.NUM_ORDEN
                       WHERE A.FEC_TRASPASO BETWEEN P_FEC_INI AND P_FEC_FIN
                         AND B.NUM_ENDOSO IN (SELECT MAX(NUM_ENDOSO)
                                                FROM PP_TMAE_BEN@DBL_RENTASV.PROTECTA.DOM
                                               WHERE NUM_POLIZA = B.NUM_POLIZA)
                         AND (CASE
                               WHEN A.COD_MONEDAPRIINF = 'NS' THEN
                                A.MTO_PRIINF / P_TC
                               ELSE
                                A.MTO_PRIINF
                             END) >= P_MONTO_UNICO
                         AND B.NUM_ORDEN = 1

                      UNION ALL*/
                      --AT
                      SELECT A.NUM_POLIZA,
                              A.FEC_ABONO AS FEC_TRASPASO,
                              A.HOR_CREA,
                              B.COD_TIPOIDENBEN,
                              B.NUM_IDENBEN,
                              B.GLS_PATBEN,
                              B.GLS_MATBEN,
                              B.GLS_NOMBEN,
                              B.GLS_NOMSEGBEN,
                              B.GLS_DIRBEN,
                              B.GLS_TELBEN2,
                              B.GLS_FONOBEN,
                              F.COD_OCUPACION,
                              DIR.COD_PAIS,
                              C.GLS_COMUNA,
                              D.GLS_PROVINCIA,
                              E.GLS_REGION,

                              C.COD_COMUNA,
                              F.CARGO,
                              A.COD_MONPRI AS MONEDA,
                              A.MTO_PRIUNI AS MONTO,
                              F.TIPO_PAGO AS INFORM,
                              'AT' AS PRODUCTO,
                              TP.NPERSON_TYP
                        FROM PP_TMAE_POLPRIREC_MUL@DBL_RENTASV.PROTECTA.DOM A
                       INNER JOIN PP_TMAE_BEN_MUL@DBL_RENTASV.PROTECTA.DOM B
                          ON A.COD_TIPPROD = B.COD_TIPPROD
                         AND A.NUM_POLIZA = B.NUM_POLIZA
                       LEFT OUTER JOIN CLIENT CL
                        ON CL.SCLIENT = TRIM(TO_CHAR(TRIM(DECODE(B.COD_TIPOIDENBEN, 1, 2, 2, 4, 5, 6, 6, 10, B.COD_TIPOIDENBEN)), '00')) || LPAD(B.NUM_IDENBEN, 12, '0')
                       LEFT OUTER JOIN TABLE5006 TP
                        ON TP.NPERSON_TYP = CL.NPERSON_TYP
                        LEFT JOIN MA_TPAR_COMUNA@DBL_RENTASV.PROTECTA.DOM C
                          ON C.COD_DIRECCION = B.COD_DIRECCION
                        LEFT JOIN MA_TPAR_PROVINCIA@DBL_RENTASV.PROTECTA.DOM D
                          ON D.COD_PROVINCIA = C.COD_PROVINCIA
                        LEFT JOIN MA_TPAR_REGION@DBL_RENTASV.PROTECTA.DOM E
                          ON E.COD_REGION = C.COD_REGION
                        LEFT JOIN PP_TMAE_BENAUX_MUL@DBL_RENTASV.PROTECTA.DOM F
                          ON B.NUM_POLIZA = F.NUM_POLIZA
                        LEFT JOIN PP_TMAE_BEN_DIRECCION@DBL_RENTASV.PROTECTA.DOM DIR

                          ON DIR.NUM_POLIZA = B.NUM_POLIZA
                         AND DIR.NUM_ENDOSO = B.NUM_ENDOSO
                         AND DIR.NUM_ORDEN = B.NUM_ORDEN

                       WHERE A.COD_TIPPROD = 'AT'
                         AND B.COD_TIPPROD = 'AT'
                         AND A.FEC_ABONO BETWEEN P_FEC_INI AND P_FEC_FIN
                         AND (CASE
                               WHEN A.COD_MONPRI = 'NS' THEN
                                A.MTO_PRIUNI / P_TC
                               ELSE
                                A.MTO_PRIUNI
                             END) >= P_MONTO_UNICO
                         AND B.NUM_ORDEN = 1
                         AND B.NUM_ENDOSO = (SELECT MAX(X.NUM_ENDOSO)
                                               FROM PP_TMAE_POLIZA_MUL@DBL_RENTASV.PROTECTA.DOM X
                                              WHERE X.COD_TIPPROD = A.COD_TIPPROD
                                                AND X.NUM_POLIZA = A.NUM_POLIZA)
                      UNION ALL
                      --RT
                      SELECT A.NUM_POLIZA,
                              A.FEC_ABONO AS FEC_TRASPASO,
                              A.HOR_CREA,
                              B.COD_TIPOIDENBEN,
                              B.NUM_IDENBEN,
                              B.GLS_PATBEN,
                              B.GLS_MATBEN,
                              B.GLS_NOMBEN,
                              B.GLS_NOMSEGBEN,
                              B.GLS_DIRBEN,
                              B.GLS_TELBEN2,
                              B.GLS_FONOBEN,
                              F.COD_OCUPACION,
                              DIR.COD_PAIS,
                              C.GLS_COMUNA,
                              D.GLS_PROVINCIA,
                              E.GLS_REGION,

                              C.COD_COMUNA,
                              F.CARGO,
                              A.COD_MONPRI AS MONEDA,
                              A.MTO_PRIUNI AS MONTO,
                              F.TIPO_PAGO AS INFORM,
                              'RT' AS PRODUCTO,
                              TP.NPERSON_TYP
                        FROM PP_TMAE_POLPRIREC_MUL@DBL_RENTASV.PROTECTA.DOM A
                       INNER JOIN PP_TMAE_BEN_MUL@DBL_RENTASV.PROTECTA.DOM B
                          ON A.COD_TIPPROD = B.COD_TIPPROD
                         AND A.NUM_POLIZA = B.NUM_POLIZA
                         LEFT OUTER JOIN CLIENT CL
                        ON CL.SCLIENT = TRIM(TO_CHAR(TRIM(DECODE(B.COD_TIPOIDENBEN, 1, 2, 2, 4, 5, 6, 6, 10, B.COD_TIPOIDENBEN)), '00')) || LPAD(B.NUM_IDENBEN, 12, '0')
                       LEFT OUTER JOIN TABLE5006 TP
                        ON TP.NPERSON_TYP = CL.NPERSON_TYP
                        LEFT JOIN MA_TPAR_COMUNA@DBL_RENTASV.PROTECTA.DOM C
                          ON C.COD_DIRECCION = B.COD_DIRECCION
                        LEFT JOIN MA_TPAR_PROVINCIA@DBL_RENTASV.PROTECTA.DOM D
                          ON D.COD_PROVINCIA = C.COD_PROVINCIA
                        LEFT JOIN MA_TPAR_REGION@DBL_RENTASV.PROTECTA.DOM E
                          ON E.COD_REGION = C.COD_REGION
                        LEFT JOIN PP_TMAE_BENAUX_MUL@DBL_RENTASV.PROTECTA.DOM F
                          ON B.NUM_POLIZA = F.NUM_POLIZA
                        LEFT JOIN PP_TMAE_BEN_DIRECCION@DBL_RENTASV.PROTECTA.DOM DIR
                          ON DIR.NUM_POLIZA = B.NUM_POLIZA
                         AND DIR.NUM_ENDOSO = B.NUM_ENDOSO
                         AND DIR.NUM_ORDEN = B.NUM_ORDEN
                        LEFT OUTER JOIN TBL_TRX_NATIONALITY_ISO NI
                          ON DIR.COD_PAIS = NI.NNATIONALITY
                         AND NI.SACTIVE = 1
                       WHERE A.COD_TIPPROD = 'RT'
                         AND B.COD_TIPPROD = 'RT'
                         AND A.FEC_ABONO BETWEEN P_FEC_INI AND P_FEC_FIN
                         AND (CASE
                               WHEN A.COD_MONPRI = 'NS' THEN
                                A.MTO_PRIUNI / P_TC
                               ELSE
                                A.MTO_PRIUNI
                             END) >= P_MONTO_UNICO
                         AND B.NUM_ORDEN = 1
                         AND B.NUM_ENDOSO = (SELECT MAX(X.NUM_ENDOSO)
                                               FROM PP_TMAE_POLIZA_MUL@DBL_RENTASV.PROTECTA.DOM X
                                              WHERE X.COD_TIPPROD = A.COD_TIPPROD
                                                AND X.NUM_POLIZA = A.NUM_POLIZA)

                      UNION ALL
                      --VR
                      SELECT A.NUM_POLIZA,
                              A.FEC_ABONO AS FEC_TRASPASO,
                              A.HOR_CREA,
                              B.COD_TIPOIDENBEN,
                              B.NUM_IDENBEN,
                              B.GLS_PATBEN,
                              B.GLS_MATBEN,
                              B.GLS_NOMBEN,
                              B.GLS_NOMSEGBEN,
                              B.GLS_DIRBEN,
                              B.GLS_TELBEN2,
                              B.GLS_FONOBEN,
                              F.COD_OCUPACION,
                              DIR.COD_PAIS,
                              C.GLS_COMUNA,
                              D.GLS_PROVINCIA,
                              E.GLS_REGION,

                              C.COD_COMUNA,
                              F.CARGO,
                              A.COD_MONPRI AS MONEDA,
                              A.MTO_PRIUNI AS MONTO,
                              F.TIPO_PAGO AS INFORM,
                              'VR' AS PRODUCTO,
                              TP.NPERSON_TYP
                        FROM PP_TMAE_POLPRIREC_MUL@DBL_RENTASV.PROTECTA.DOM A
                       INNER JOIN PP_TMAE_BEN_MUL@DBL_RENTASV.PROTECTA.DOM B
                          ON A.COD_TIPPROD = B.COD_TIPPROD
                         AND A.NUM_POLIZA = B.NUM_POLIZA
                         LEFT OUTER JOIN CLIENT CL
                        ON CL.SCLIENT = TRIM(TO_CHAR(TRIM(DECODE(B.COD_TIPOIDENBEN, 1, 2, 2, 4, 5, 6, 6, 10, B.COD_TIPOIDENBEN)), '00')) || LPAD(B.NUM_IDENBEN, 12, '0')
                       LEFT OUTER JOIN TABLE5006 TP
                        ON TP.NPERSON_TYP = CL.NPERSON_TYP
                        LEFT JOIN MA_TPAR_COMUNA@DBL_RENTASV.PROTECTA.DOM C
                          ON C.COD_DIRECCION = B.COD_DIRECCION
                        LEFT JOIN MA_TPAR_PROVINCIA@DBL_RENTASV.PROTECTA.DOM D
                          ON D.COD_PROVINCIA = C.COD_PROVINCIA
                        LEFT JOIN MA_TPAR_REGION@DBL_RENTASV.PROTECTA.DOM E
                          ON E.COD_REGION = C.COD_REGION
                        LEFT JOIN PP_TMAE_BENAUX_MUL@DBL_RENTASV.PROTECTA.DOM F
                          ON B.NUM_POLIZA = F.NUM_POLIZA
                        LEFT JOIN PP_TMAE_BEN_DIRECCION@DBL_RENTASV.PROTECTA.DOM DIR
                          ON DIR.NUM_POLIZA = B.NUM_POLIZA
                         AND DIR.NUM_ENDOSO = B.NUM_ENDOSO
                         AND DIR.NUM_ORDEN = B.NUM_ORDEN
                        LEFT JOIN EQUI_UBIGEO EQUI
                          ON TO_NUMBER(EQUI.COD_UBI_DIS) = C.COD_COMUNA
                         AND EQUI.COD_CLI = '11111111111111'
                        LEFT OUTER JOIN TBL_TRX_NATIONALITY_ISO NI
                          ON DIR.COD_PAIS = NI.NNATIONALITY
                         AND NI.SACTIVE = 1

                       WHERE A.COD_TIPPROD = 'VR'
                         AND B.COD_TIPPROD = 'VR'
                         AND A.FEC_ABONO BETWEEN P_FEC_INI AND P_FEC_FIN
                         AND (CASE
                               WHEN A.COD_MONPRI = 'NS' THEN
                                A.MTO_PRIUNI / P_TC
                               ELSE
                                A.MTO_PRIUNI
                             END) >= P_MONTO_UNICO
                         AND B.NUM_ORDEN = 1
                         AND B.NUM_ENDOSO = (SELECT MAX(X.NUM_ENDOSO)
                                               FROM PP_TMAE_POLIZA_MUL@DBL_RENTASV.PROTECTA.DOM X
                                              WHERE X.COD_TIPPROD = A.COD_TIPPROD
                                                AND X.NUM_POLIZA = A.NUM_POLIZA)

                      ) P
                LEFT JOIN EQUI_UBIGEO EQUI
                  ON TO_NUMBER(EQUI.COD_UBI_DIS) = P.COD_COMUNA
                 AND EQUI.COD_CLI = '11111111111111'
                LEFT OUTER JOIN TBL_TRX_NATIONALITY_ISO NI
                  ON NI.NNATIONALITY = P.COD_PAIS
                 AND NI.SACTIVE = 1
              /*LEFT OUTER JOIN TBL_CONFIG_OCUP_SBS COS
              ON COS.NIDOCUPACION = A.NSPECIALITY
              AND COS.SORIGEN_BD = 'TIME'
              AND COS.SACTIVE = '1'*/

              ) Z;

  END RENTAS_MAYORES;

  PROCEDURE PRIMAS_MAYORES_UNICA(P_OPE    NUMBER,
                                 P_TC     NUMBER,
                                 P_MONTO  NUMBER,
                                 P_FECINI VARCHAR2,
                                 P_FECFIN VARCHAR2,
                                 C_TABLE  OUT MYCURSOR) IS
  BEGIN
    OPEN C_TABLE FOR

      SELECT

       TO_CHAR(ROWNUM, '00000000') AS FILA, --1-vb.801
       '001 ' AS OFICINA, --2
       TO_CHAR(ROWNUM, '00000000') AS OPERACION, --3-vb.802
       RPAD(TRIM(TRIM(TO_CHAR(K.DBILLDATE, 'YYYYMM')) || TRIM(TO_CHAR(DECODE(TRIM(K.NPOLICY), '', 0, K.NPOLICY), '0000')) || TRIM(TO_CHAR(TRIM(K.NCERTIF), '0000000000'))), 20) AS INTERNO, --4
       'U' AS MODALIDAD, --5-vb.827
       '150141' AS OPE_UBIGEO, --6
       RPAD(DECODE(K.DBILLDATE, '', ' ', TO_CHAR(K.DBILLDATE, 'YYYYMMDD')), 8) AS OPE_FECHA, --7
       RPAD(DECODE(K.DCOMPDATE, '', ' ', TO_CHAR(K.DCOMPDATE, 'HH24MISS')), 6) AS OPE_HORA, --8
       RPAD(' ', 10) AS EJE_RELACION, --vb.835
       RPAD(' ', 1) AS EJE_CONDICION, --vb.836
       RPAD(' ', 1) AS EJE_TIPPER, --vb
       RPAD(' ', 1) AS EJE_TIPDOC, --vb
       RPAD(' ', 12) AS EJE_NUMDOC, --vb
       RPAD(' ', 11) AS EJE_NUMRUC, --vb
       RPAD(' ', 40) AS EJE_APEPAT, --vb
       RPAD(' ', 40) AS EJE_APEMAT, --vb
       RPAD(' ', 40) AS EJE_NOMBRES, --vb
       RPAD(' ', 4) AS EJE_OCUPACION, --vb
       --'' AS EJE_CIIU, --vb.845
       RPAD(' ', 6) AS EJE_PAIS,
       --'' AS EJE_DESCIIU, --vb.846
       RPAD(' ', 104) AS EJE_CARGO, --vb.847
       RPAD(' ', 2) AS EJE_PEP,
       RPAD(' ', 150) AS EJE_DOMICILIO, --vb.848
       RPAD(' ', 2) AS EJE_DEPART, --vb.849
       RPAD(' ', 2) AS EJE_PROV, --vb.850
       RPAD(' ', 2) AS EJE_DIST, --vb.851
       RPAD(' ', 40) AS EJE_TELEFONO, --vb.852

       RPAD('1', 1) AS ORD_RELACION, --27-vb.735
       RPAD('1', 1) AS ORD_CONDICION, --28-vb.736
       RPAD('1', 1) AS ORD_TIPPER, --29-
       RPAD(DECODE(SUBSTR(CLASEG.SCLIENT, 1, 2), '02', '1', '01', '9', ' '), 1) AS ORD_TIPDOC, --30-vb.738
       RPAD(DECODE(SUBSTR(CLASEG.SCLIENT, 1, 2), '02', RIGHT(CLASEG.SCLIENT, 8), '01', ' ', RIGHT(CLASEG.SCLIENT, 12)), 12) AS ORD_NUMDOC, --31-vb.671
       RPAD(DECODE(SUBSTR(CLASEG.SCLIENT, 1, 2), '02', CLASEG.STAX_CODE, '01', RIGHT(CLASEG.SCLIENT, 11), ' '), 11) AS ORD_NUMRUC, --32
       RPAD(REPLACE(REPLACE(REPLACE(DECODE(SUBSTR(CLASEG.SCLIENT, 1, 2), '02', CLASEG.SLASTNAME, '01', CLASEG.SCLIENAME, ' '), '?', '#'), 'Ñ', '#'), 'ñ', '#'), 120) AS ORD_APEPAT, --33

       RPAD(REPLACE(REPLACE(REPLACE(DECODE(CLASEG.SLASTNAME2, '', ' ', CLASEG.SLASTNAME2), '?', '#'), 'Ñ', '#'), 'ñ', '#'), 40) AS ORD_APEMAT, --34
       RPAD(REPLACE(REPLACE(REPLACE(DECODE(CLASEG.SFIRSTNAME, '', ' ', CLASEG.SFIRSTNAME), '?', '#'), 'Ñ', '#'), 'ñ', '#'), 40) AS ORD_NOMBRES, --35

       RPAD(DECODE(CLASEG.COD_OCUPACION_ASEGU, '', ' ', CLASEG.COD_OCUPACION_ASEGU), 4) AS ORD_OCUPACION, --36-vb.744
       RPAD('PE', 6) AS ORD_PAIS, --37
       --RPAD('' AS Ord_CIIU,--37-YA NO SE SOLICITA
       --RPAD('' AS Ord_DesCIIU,--38-YA NO SE SOLICITA
       RPAD(' ', 104) AS ORD_CARGO, --38-NO ES OBLIGATORIO.vb747
       RPAD(' ', 2) AS ORD_PEP, --39-NO ES OBLIGATORIO
       CLASEG.SSTREET AS ORD_DIRECCION,
       RPAD(REPLACE(REPLACE(REPLACE(DECODE(CLASEG.SSTREET, '', ' ', TRIM(CLASEG.SSTREET)) || ' ' || TRIM(CLASEG.DISTRITO) || ' ' || TRIM(CLASEG.PROV) || ' ' || TRIM(CLASEG.DEPART), '?', '#'),
                            'Ñ',
                            '#'),
                    'ñ',
                    '#'),
            150) AS ORD_DOMICILIO, --40
       TO_NUMBER(CLASEG.COD_UBI_CLI) AS UBIGEO,
       RPAD(DECODE(CLASEG.NMUNICIPALITY, '', '15', SUBSTR(TRIM(TO_CHAR((CLASEG.COD_UBI_CLI), '000000')), 1, 2)), 2) AS ORD_DEPART, --41
       RPAD(DECODE(CLASEG.NMUNICIPALITY, '', '01', SUBSTR(TRIM(TO_CHAR((CLASEG.COD_UBI_CLI), '000000')), 3, 2)), 2) AS ORD_PROV, --42
       RPAD(DECODE(CLASEG.NMUNICIPALITY, '', '41', SUBSTR(TRIM(TO_CHAR((CLASEG.COD_UBI_CLI), '000000')), 5, 2)), 2) AS ORD_DIST, --43
       RPAD(DECODE(CLASEG.SPHONE, '', ' ', CLASEG.SPHONE), 40) AS ORD_TELEFONO, --44
       RPAD('1', 1) AS BEN_RELACION, --45
       RPAD('1', 1) AS BEN_CONDICION, --46
       RPAD(DECODE(SUBSTR(CLBENE.SCLIENT, 1, 2), '02', '2', '01', '3', ' '), 1) AS BEN_TIP_PER, --47
       RPAD(DECODE(SUBSTR(CLBENE.SCLIENT, 1, 2), '02', '1', '01', '9', ' '), 1) AS BEN_TIP_DOC, --48
       RPAD(DECODE(SUBSTR(CLBENE.SCLIENT, 1, 2), '02', RIGHT(CLBENE.SCLIENT, 8), '01', ' ', RIGHT(CLBENE.SCLIENT, 12)), 12) AS BEN_NUM_DOC, --49
       RPAD(DECODE(SUBSTR(CLBENE.SCLIENT, 1, 2), '02', CLBENE.STAX_CODE, '01', RIGHT(CLBENE.SCLIENT, 11), ' '), 11) AS BEN_NUM_RUC, --50
       RPAD(REPLACE(REPLACE(REPLACE(DECODE(SUBSTR(CLBENE.SCLIENT, 1, 2), '02', CLBENE.SLASTNAME, '01', CLBENE.SCLIENAME, ' '), '?', '#'), 'Ñ', '#'), 'ñ', '#'), 120) AS BEN_APEPAT, --51
       RPAD(REPLACE(REPLACE(REPLACE(DECODE(CLBENE.SLASTNAME2, '', ' ', CLBENE.SLASTNAME2), '?', '#'), 'Ñ', '#'), 'ñ', '#'), 40) AS BEN_APEMAT, --52
       RPAD(REPLACE(REPLACE(REPLACE(DECODE(CLBENE.SFIRSTNAME, '', ' ', CLBENE.SFIRSTNAME), '?', '#'), 'Ñ', '#'), 'ñ', '#'), 40) AS BEN_NOMBRES, --53

       RPAD(DECODE(CLBENE.COD_OCUPACION_ASEGU, '', ' ', CLASEG.COD_OCUPACION_ASEGU), 4) AS BEN_OCUPACION, --54
       --RPAD('' AS Ben_CIIU,--55-YA NO SE SOLICITA
       RPAD('PE', 6) /*tb66.sdescript*/ AS BEN_PAIS, --55
       --RPAD('' AS Ben_Des_CIIU,--56-YA NO SE SOLICITA
       RPAD(' ', 104) AS BEN_CARGO, --56-NO ES OBLIGATORIO
       RPAD(' ', 2) AS BEN_PEP, --57-NO ES OBLIGATORIO
       CLBENE.SSTREET AS DIRECCION_B,
       RPAD(REPLACE(REPLACE(REPLACE(DECODE(CLBENE.SSTREET, '', ' ', TRIM(CLBENE.SSTREET)) || ' ' || TRIM(CLBENE.DISTRITO) || ' ' || TRIM(CLBENE.PROV) || ' ' || TRIM(CLBENE.DEPART), '?', '#'),
                            'Ñ',
                            '#'),
                    'ñ',
                    '#'),
            150) AS BEN_DOMICILIO, --58
       TO_NUMBER(CLBENE.COD_UBI_CLI) AS BEN_UBIGEO,
       RPAD(DECODE(CLBENE.NMUNICIPALITY, '', '15', SUBSTR(TRIM(TO_CHAR((CLBENE.COD_UBI_CLI), '000000')), 1, 2)), 2) AS BEN_DEPART, --59
       RPAD(DECODE(CLBENE.NMUNICIPALITY, '', '01', SUBSTR(TRIM(TO_CHAR((CLBENE.COD_UBI_CLI), '000000')), 3, 2)), 2) AS BEN_PROV, --60
       RPAD(DECODE(CLBENE.NMUNICIPALITY, '', '41', SUBSTR(TRIM(TO_CHAR((CLBENE.COD_UBI_CLI), '000000')), 5, 2)), 2) AS BEN_DIST, --61
       RPAD(DECODE(CLBENE.SPHONE, '', ' ', CLBENE.SPHONE), 40) AS BEN_TELEFONO, --62
       RPAD('2', 1) AS DAT_TIPFON, --63
       RPAD('34', 2) AS DAT_TIPOPE, --64
       RPAD(' ', 40) AS DAT_DESOPE, --65
       RPAD(' ', 80) AS DAT_ORIFON, --66
       RPAD(DECODE(K.NCURRENCY, '1', 'PEN', 'USD'), 3) AS DAT_MONOPE, --67
       RPAD(' ', 3) AS DAT_MONOPE_A, --68
       RPAD(TO_CHAR(CAST((K.NAMOUNT) AS NUMBER(15, 2)), '000000000000000.00'), 29) AS DAT_MTOOPE, --MTO_PENSIONGAR , MTO_PRIUNI 69
       RPAD(' ', 30) AS DAT_MTOOPEA, --MTO_PENSIONGAR , MTO_PRIUNI 70
       RPAD(' ', 5) AS DAT_COD_ENT_INVO, --71--NO OBLIGATORIO
       RPAD('1', 1) AS DAT_COD_TIP_CTAO, --72--NO OBLIGATORIO
       RPAD('002193116232365', 20) AS DAT_COD_CTAO, --73--NO OBLIGATORIO
       RPAD(' ', 150) AS DAT_ENT_FNC_EXTO, --74--NO OBLIGATORIO
       RPAD(' ', 5) AS DAT_COD_ENT_INVB, --75--NO OBLIGATORIO
       RPAD('1', 1) AS DAT_COD_TIP_CTAB, --76--NO OBLIGATORIO
       RPAD(' ', 20) AS DAT_COD_CTAB, --77--NO OBLIGATORIO
       RPAD(' ', 150) AS DAT_ENT_FNC_EXTB, --78--NO OBLIGATORIO
       RPAD('1', 1) AS DAT_ALCANCE, --79
       RPAD(' ', 2) AS DAT_COD_PAISO, --80--ALCANCE 1, ESTE CAMPO VA EN BLANCO
       RPAD(' ', 2) AS DAT_COD_PAISD, --81--ALCANCE 1, ESTE CAMPO VA EN BLANCO
       RPAD('2', 1) AS DAT_INTOPE, --82--NO OBLIGATORIO
       RPAD('2', 1) AS DAT_FORMA, --83
       RPAD(DECODE(K.SDESCRIPT, '', ' ', K.SDESCRIPT), 40) AS DAT_INFORM, --84--NO OBLIGATORIO
       'PRI' AS ORIGEN

        FROM (SELECT
               /*+INDEX(P XPKPOLICY) INDEX(CT XPKCERTIFICAT) INDEX(CL XPKCLIENT) INDEX(M XPKPRODMASTER)*/
                A.NPRODUCT,
                A.NPOLICY,
                M.SDESCRIPT AS DESCRIPCIONPRODUCTO,
                C.DBILLDATE,
                A.NCERTIF,
                CT.SCLIENT AS BENEFICIARIO,
                CL.SCLIENAME,
                C.NAMOUNT,
                C.NCURRENCY,
                P_TC AS TIPO_CAMBIO,
                TO_CHAR(DECODE(C.NCURRENCY, 1, C.NAMOUNT, 0)) AS SOLES,
                TO_CHAR(DECODE(C.NCURRENCY, 2, A.NPREMIUM, ROUND(A.NPREMIUM / P_TC, 4))) AS DOLARES,
                B.DCOMPDATE,
                A.SCLIENT AS ASEGURADO,
                T.SDESCRIPT,
                '' AS HOR_CREA
                 FROM LIFE       A,
                      CERTIFICAT CT,
                      PREMIUM    B,
                      BILLS      C,
                      POLICY     P,
                      TABLE36    T,
                      PRODMASTER M,
                      TABLE5564  E,
                      CLIENT     CL

                WHERE

                CT.SCERTYPE = A.SCERTYPE
             AND CT.NBRANCH = A.NBRANCH
             AND CT.NPRODUCT = A.NPRODUCT
             AND CT.NPOLICY = A.NPOLICY
             AND CT.NCERTIF = A.NCERTIF
             AND B.NRECEIPT = A.NRECEIPT
             AND C.NINSUR_AREA = B.NINSUR_AREA
             AND C.SBILLING = '1'
             AND C.SBILLTYPE = B.SBILLTYPE
             AND C.NBILLNUM = B.NBILLNUM
             AND P.SCERTYPE = A.SCERTYPE
             AND P.NBRANCH = A.NBRANCH
             AND P.NPRODUCT = A.NPRODUCT
             AND P.NPOLICY = A.NPOLICY
             AND P.NPAYFREQ = T.NPAYFREQ
             AND A.NPRODUCT = M.NPRODUCT
             AND C.NBILLSTAT = E.NBILLSTAT
             AND A.SCLIENT = CL.SCLIENT
             AND A.SCERTYPE = '2'
             AND A.NBRANCH = 1
             AND A.NCERTIF > 0
             AND P.NPAYFREQ = 6
             AND TRUNC(C.DBILLDATE) BETWEEN P_FECINI AND P_FECFIN
             AND C.NBILLSTAT <> 2
             AND DECODE(C.NCURRENCY, 2, A.NPREMIUM, ROUND(A.NPREMIUM / P_TC, 4)) >= P_MONTO
             AND NOT B.NPRODUCT IN (117, 120, 1000)

               UNION ALL

               SELECT
               /*+INDEX(P XPKPOLICY) INDEX(CT XPKCERTIFICAT) INDEX(CL XPKCLIENT) INDEX(M XPKPRODMASTER)*/
                A.NPRODUCT,
                A.NPOLICY,
                M.SDESCRIPT AS DESCRIPCIONPRODUCTO,
                C.DBILLDATE,
                A.NCERTIF,
                CT.SCLIENT AS BENEFICIARIO,
                CL.SCLIENAME,
                C.NAMOUNT,
                C.NCURRENCY,
                P_TC AS TIPO_CAMBIO,
                TO_CHAR(DECODE(C.NCURRENCY, 1, C.NAMOUNT, 0)) AS SOLES,
                TO_CHAR(DECODE(C.NCURRENCY, 2, A.NPREMIUM, ROUND(A.NPREMIUM / P_TC, 4))) AS DOLARES,
                B.DCOMPDATE,
                A.SCLIENT AS ASEGURADO,
                T.SDESCRIPT,
                '' AS HOR_CREA
                 FROM LIFE A,
                      CERTIFICAT CT,
                      PREMIUM B,
                      BILLS C,
                      POLICY P,
                      TABLE36 T,
                      TABLE5564 E,
                      PRODMASTER M,
                      CLIENT CL,
                      (SELECT B.SCLIENT,
                              B.NRECEIPT,
                              SUM(DECODE(A.NCURRENCY, 2, A.NAMOUNT, ROUND(A.NAMOUNT / P_TC, 4))) AS MONTO
                         FROM BILLS A
                        INNER JOIN PREMIUM B
                           ON B.NINSUR_AREA = A.NINSUR_AREA
                          AND B.SBILLTYPE = A.SBILLTYPE
                          AND B.NBILLNUM = A.NBILLNUM
                        WHERE B.NPRODUCT IN (117, 120)
                          AND A.NBILLSTAT <> 2
                          AND TRUNC(A.DBILLDATE) BETWEEN P_FECINI AND P_FECFIN
                        GROUP BY B.SCLIENT,
                                 B.NRECEIPT) X

                WHERE CT.SCERTYPE = A.SCERTYPE
                  AND CT.NBRANCH = A.NBRANCH
                  AND CT.NPRODUCT = A.NPRODUCT
                  AND CT.NPOLICY = A.NPOLICY
                  AND CT.NCERTIF = A.NCERTIF
                  AND A.NRECEIPT = B.NRECEIPT
                  AND C.NINSUR_AREA = B.NINSUR_AREA
                  AND C.SBILLING = '1'
                  AND C.SBILLTYPE = B.SBILLTYPE
                  AND C.NBILLNUM = B.NBILLNUM
                  AND P.SCERTYPE = A.SCERTYPE
                  AND P.NBRANCH = A.NBRANCH
                  AND P.NPRODUCT = A.NPRODUCT
                  AND P.NPOLICY = A.NPOLICY
                  AND P.NPAYFREQ = T.NPAYFREQ
                  AND A.NPRODUCT = M.NPRODUCT
                  AND C.NBILLSTAT = E.NBILLSTAT
                  AND A.SCLIENT = CL.SCLIENT
                  AND X.SCLIENT = CT.SCLIENT
                  AND X.NRECEIPT = B.NRECEIPT
                  AND X.MONTO > P_MONTO

                  AND A.SCERTYPE = '2'
                  AND A.NBRANCH = 1
                  AND A.NCERTIF > 0
                  AND P.NPAYFREQ = 6
                  AND TRUNC(C.DBILLDATE) BETWEEN P_FECINI AND P_FECFIN
                  AND C.NBILLSTAT <> 2

               UNION ALL

               SELECT
               /*+INDEX(P XPKPOLICY) INDEX(CT XPKCERTIFICAT) INDEX(CL XPKCLIENT) INDEX(M XPKPRODMASTER)*/
                A.NPRODUCT,
                A.NPOLICY,
                M.SDESCRIPT AS DESCRIPCIONPRODUCTO,
                C.DBILLDATE,
                A.NCERTIF,
                CT.SCLIENT AS BENEFICIARIO,
                CL.SCLIENAME,
                C.NAMOUNT,
                C.NCURRENCY,
                P_TC AS TIPO_CAMBIO,
                TO_CHAR(DECODE(C.NCURRENCY, 1, C.NAMOUNT, 0)) AS SOLES,
                TO_CHAR(DECODE(C.NCURRENCY, 2, C.NAMOUNT, ROUND(C.NAMOUNT / P_TC, 4))) AS DOLARES,
                B.DCOMPDATE,
                A.SCLIENT AS ASEGURADO,
                T.SDESCRIPT,
                '' AS HOR_CREA
                 FROM LIFE_HIS A
                INNER JOIN CERTIFICAT CT
                   ON CT.SCERTYPE = A.SCERTYPE
                  AND CT.NBRANCH = A.NBRANCH
                  AND CT.NPRODUCT = A.NPRODUCT
                  AND CT.NPOLICY = A.NPOLICY
                  AND CT.NCERTIF = A.NCERTIF
                INNER JOIN PREMIUM B
                   ON B.NRECEIPT = A.NRECEIPT
                INNER JOIN BILLS C
                   ON C.NINSUR_AREA = B.NINSUR_AREA
                  AND C.SBILLING = '1'
                  AND C.SBILLTYPE = B.SBILLTYPE
                  AND C.NBILLNUM = B.NBILLNUM
                INNER JOIN POLICY P
                   ON P.SCERTYPE = A.SCERTYPE
                  AND P.NBRANCH = A.NBRANCH
                  AND P.NPRODUCT = A.NPRODUCT
                  AND P.NPOLICY = A.NPOLICY
                INNER JOIN TABLE36 T
                   ON P.NPAYFREQ = T.NPAYFREQ
                INNER JOIN PRODMASTER M
                   ON A.NPRODUCT = M.NPRODUCT
                INNER JOIN TABLE5564 E
                   ON C.NBILLSTAT = E.NBILLSTAT
                INNER JOIN CLIENT CL
                   ON A.SCLIENT = CL.SCLIENT
                WHERE A.SCERTYPE = '2'
                  AND A.NBRANCH = 1
                  AND A.NCERTIF > 0
                  AND P.NPAYFREQ = 6
                  AND TRUNC(C.DBILLDATE) BETWEEN P_FECINI AND P_FECFIN
                  AND C.NBILLSTAT <> 2
                  AND DECODE(C.NCURRENCY, 2, C.NAMOUNT, ROUND(C.NAMOUNT / P_TC, 4)) >= P_MONTO
                  AND NOT B.NPRODUCT IN (117, 120, 1000)

               UNION ALL
               SELECT
               /*+INDEX(P XPKPOLICY) INDEX(CT XPKCERTIFICAT) INDEX(CL XPKCLIENT) INDEX(M XPKPRODMASTER)*/
                CT.NPRODUCT,
                CT.NPOLICY,
                M.SDESCRIPT AS DESCRIPCIONPRODUCTO,
                C.DBILLDATE,
                CT.NCERTIF,
                CT.SCLIENT AS BENEFICIARIO,
                CL.SCLIENAME,
                C.NAMOUNT,
                C.NCURRENCY,
                P_TC AS TIPO_CAMBIO,
                TO_CHAR(DECODE(C.NCURRENCY, 1, C.NAMOUNT, 0)) AS SOLES,
                TO_CHAR(DECODE(C.NCURRENCY, 2, B.NPREMIUM, ROUND(B.NPREMIUM / P_TC, 4))) AS DOLARES,
                B.DCOMPDATE,
                B.SCLIENT AS ASEGURADO,
                T.SDESCRIPT,
                '' AS HOR_CREA
                 FROM CERTIFICAT CT
                INNER JOIN PREMIUM B
                   ON B.SCERTYPE = CT.SCERTYPE
                  AND B.NBRANCH = CT.NBRANCH
                  AND B.NPRODUCT = CT.NPRODUCT
                  AND B.NPOLICY = CT.NPOLICY --and ct.nreceipt=b.nreceipt
                INNER JOIN BILLS C
                   ON C.NINSUR_AREA = B.NINSUR_AREA
                  AND C.SBILLING = '1'
                  AND C.SBILLTYPE = B.SBILLTYPE
                  AND C.NBILLNUM = B.NBILLNUM
                INNER JOIN POLICY P
                   ON P.SCERTYPE = B.SCERTYPE
                  AND P.NBRANCH = B.NBRANCH
                  AND P.NPRODUCT = B.NPRODUCT
                  AND P.NPOLICY = B.NPOLICY
                INNER JOIN TABLE36 T
                   ON P.NPAYFREQ = T.NPAYFREQ
                INNER JOIN PRODMASTER M
                   ON B.NPRODUCT = M.NPRODUCT
                INNER JOIN TABLE5564 E
                   ON C.NBILLSTAT = E.NBILLSTAT
                INNER JOIN CLIENT CL
                   ON B.SCLIENT = CL.SCLIENT
                WHERE CT.SCERTYPE = '2'
                  AND CT.NBRANCH = 1
                  AND CT.NCERTIF > 0
                  AND P.NPAYFREQ = 6
                  AND TRUNC(C.DBILLDATE) BETWEEN P_FECINI AND P_FECFIN
                  AND C.NBILLSTAT <> 2
                  AND DECODE(C.NCURRENCY, 2, B.NPREMIUM, ROUND(B.NPREMIUM / P_TC, 4)) >= P_MONTO
                  AND B.NPRODUCT IN (1000)

               ) K

       INNER JOIN (SELECT
                   /*+INDEX(EQUI SYS_C00167813) INDEX(c XPKPROVINCE) INDEX(d XPKTAB_LOCAT) INDEX(e XPKMUNICIPALITY) INDEX(cc CLIENT_COMPLEMENT_PK)*/
                    A.NPERSON_TYP,
                    A.SCLIENAME,
                    A.SFIRSTNAME,
                    A.SLASTNAME,
                    A.SLASTNAME2,
                    A.NQ_CHILD,
                    ADR.SSTREET,
                    C.SDESCRIPT              AS DEPART,
                    D.SDESCRIPT              AS PROV,
                    E.SDESCRIPT              AS DISTRITO,
                    PH.SPHONE,
                    A.STAX_CODE,
                    E.NMUNICIPALITY,
                    CC.CTA_BCRIA_CONTRATANTE,
                    CC.COD_EMP,
                    CC.CIIU_CONTRATANTE,
                    CC.COD_OCUPACION_ASEGU,
                    A.SCLIENT,
                    EQUI.COD_UBI_CLI
                     FROM CLIENT A

                     LEFT OUTER JOIN (SELECT SCLIENT,
                                            NLOCAL,
                                            NPROVINCE,
                                            NMUNICIPALITY,
                                            NCOUNTRY,
                                            SKEYADDRESS,
                                            SSTREET
                                       FROM ADDRESS ADRR
                                      WHERE ADRR.NRECOWNER = 2
                                        AND ADRR.SRECTYPE = 2
                                        AND ADRR.DNULLDATE IS NULL
                                        AND TRIM(ADRR.SKEYADDRESS) || TO_CHAR(ADRR.DCOMPDATE, 'YYYYMMDDHH24MMSS') || TO_CHAR(ADRR.DEFFECDATE, 'YYYYMMDDHH24MMSS') =
                                            (SELECT /*+INDEX (AT XIF2110ADDRESS)*/
                                              MAX(TRIM(AT.SKEYADDRESS) || TO_CHAR(AT.DCOMPDATE, 'YYYYMMDDHH24MMSS') || TO_CHAR(AT.DEFFECDATE, 'YYYYMMDDHH24MMSS'))
                                               FROM ADDRESS AT
                                              WHERE AT.SCLIENT = ADRR.SCLIENT
                                                AND AT.NRECOWNER = 2
                                                AND AT.SRECTYPE = 2
                                                AND AT.DNULLDATE IS NULL)

                                     ) ADR
                       ON ADR.SCLIENT = A.SCLIENT

                     LEFT OUTER JOIN (SELECT
                                     /*+INDEX (PHR IDX_PHONE_1)*/
                                      SPHONE,
                                      NKEYPHONES,
                                      SKEYADDRESS,
                                      DCOMPDATE,
                                      DEFFECDATE
                                       FROM PHONES PHR
                                      WHERE PHR.NRECOWNER = 2
                                        AND PHR.DNULLDATE IS NULL
                                        AND TRIM(PHR.SKEYADDRESS) || PHR.NKEYPHONES || TO_CHAR(PHR.DCOMPDATE, 'YYYYMMDDHH24MMSS') || TO_CHAR(PHR.DEFFECDATE, 'YYYYMMDDHH24MMSS') =
                                            (SELECT /*+INDEX (PHT IDX_PHONE_1)*/
                                              MAX(TRIM(PHT.SKEYADDRESS) || PHT.NKEYPHONES || TO_CHAR(PHT.DCOMPDATE, 'YYYYMMDDHH24MMSS') || TO_CHAR(PHT.DEFFECDATE, 'YYYYMMDDHH24MMSS'))
                                               FROM PHONES PHT
                                              WHERE SUBSTR(PHT.SKEYADDRESS, 2, 14) = SUBSTR(PHR.SKEYADDRESS, 2, 14)
                                                AND PHT.NRECOWNER = 2
                                                AND PHT.DNULLDATE IS NULL)) PH
                       ON SUBSTR(PH.SKEYADDRESS, 2, 14) = A.SCLIENT

                     LEFT JOIN PROVINCE C
                       ON C.NPROVINCE = ADR.NPROVINCE
                     LEFT JOIN TAB_LOCAT D
                       ON D.NLOCAL = ADR.NLOCAL
                     LEFT JOIN MUNICIPALITY E
                       ON E.NMUNICIPALITY = ADR.NMUNICIPALITY
                     LEFT JOIN CLIENT_COMPLEMENT CC
                       ON A.SCLIENT = CC.SCLIENT
                     LEFT OUTER JOIN EQUI_UBIGEO EQUI
                       ON TO_NUMBER(EQUI.COD_UBI_DIS) = E.NMUNICIPALITY
                      AND EQUI.COD_CLI = '11111111111111') CLASEG
          ON CLASEG.SCLIENT = K.ASEGURADO

       INNER JOIN (SELECT
                   /*+INDEX(EQUI SYS_C00167813) INDEX(c XPKPROVINCE) INDEX(d XPKTAB_LOCAT) INDEX(e XPKMUNICIPALITY) INDEX(cc CLIENT_COMPLEMENT_PK)*/
                    A.NPERSON_TYP,
                    A.SCLIENAME,
                    A.SFIRSTNAME,
                    A.SLASTNAME,
                    A.SLASTNAME2,
                    A.NQ_CHILD,
                    ADR.SSTREET,
                    C.SDESCRIPT              AS DEPART,
                    D.SDESCRIPT              AS PROV,
                    E.SDESCRIPT              AS DISTRITO,
                    PH.SPHONE,
                    A.STAX_CODE,
                    E.NMUNICIPALITY,
                    CC.CTA_BCRIA_CONTRATANTE,
                    CC.COD_EMP,
                    CC.CIIU_CONTRATANTE,
                    CC.COD_OCUPACION_ASEGU,
                    A.SCLIENT,
                    EQUI.COD_UBI_CLI
                     FROM CLIENT A

                     LEFT OUTER JOIN (SELECT SCLIENT,
                                            NLOCAL,
                                            NPROVINCE,
                                            NMUNICIPALITY,
                                            NCOUNTRY,
                                            SKEYADDRESS,
                                            SSTREET
                                       FROM ADDRESS ADRR
                                      WHERE ADRR.NRECOWNER = 2
                                        AND ADRR.SRECTYPE = 2
                                        AND ADRR.DNULLDATE IS NULL
                                        AND TRIM(ADRR.SKEYADDRESS) || TO_CHAR(ADRR.DCOMPDATE, 'YYYYMMDDHH24MMSS') || TO_CHAR(ADRR.DEFFECDATE, 'YYYYMMDDHH24MMSS') =
                                            (SELECT /*+INDEX (AT XIF2110ADDRESS)*/
                                              MAX(TRIM(AT.SKEYADDRESS) || TO_CHAR(AT.DCOMPDATE, 'YYYYMMDDHH24MMSS') || TO_CHAR(AT.DEFFECDATE, 'YYYYMMDDHH24MMSS'))
                                               FROM ADDRESS AT
                                              WHERE AT.SCLIENT = ADRR.SCLIENT
                                                AND AT.NRECOWNER = 2
                                                AND AT.SRECTYPE = 2
                                                AND AT.DNULLDATE IS NULL)

                                     ) ADR
                       ON ADR.SCLIENT = A.SCLIENT

                     LEFT OUTER JOIN (SELECT
                                     /*+INDEX (PHR IDX_PHONE_1)*/
                                      SPHONE,
                                      NKEYPHONES,
                                      SKEYADDRESS,
                                      DCOMPDATE,
                                      DEFFECDATE
                                       FROM PHONES PHR
                                      WHERE PHR.NRECOWNER = 2
                                        AND PHR.DNULLDATE IS NULL
                                        AND TRIM(PHR.SKEYADDRESS) || PHR.NKEYPHONES || TO_CHAR(PHR.DCOMPDATE, 'YYYYMMDDHH24MMSS') || TO_CHAR(PHR.DEFFECDATE, 'YYYYMMDDHH24MMSS') =
                                            (SELECT /*+INDEX (PHT IDX_PHONE_1)*/
                                              MAX(TRIM(PHT.SKEYADDRESS) || PHT.NKEYPHONES || TO_CHAR(PHT.DCOMPDATE, 'YYYYMMDDHH24MMSS') || TO_CHAR(PHT.DEFFECDATE, 'YYYYMMDDHH24MMSS'))
                                               FROM PHONES PHT
                                              WHERE SUBSTR(PHT.SKEYADDRESS, 2, 14) = SUBSTR(PHR.SKEYADDRESS, 2, 14)
                                                AND PHT.NRECOWNER = 2
                                                AND PHT.DNULLDATE IS NULL)

                                     ) PH
                       ON SUBSTR(PH.SKEYADDRESS, 2, 14) = A.SCLIENT

                     LEFT JOIN PROVINCE C
                       ON C.NPROVINCE = ADR.NPROVINCE
                     LEFT JOIN TAB_LOCAT D
                       ON D.NLOCAL = ADR.NLOCAL
                     LEFT JOIN MUNICIPALITY E
                       ON E.NMUNICIPALITY = ADR.NMUNICIPALITY
                     LEFT JOIN CLIENT_COMPLEMENT CC
                       ON A.SCLIENT = CC.SCLIENT
                     LEFT OUTER JOIN EQUI_UBIGEO EQUI
                       ON TO_NUMBER(EQUI.COD_UBI_DIS) = E.NMUNICIPALITY
                      AND EQUI.COD_CLI = '11111111111111') CLBENE
          ON CLBENE.SCLIENT = K.BENEFICIARIO;

  END PRIMAS_MAYORES_UNICA;

  PROCEDURE PRIMAS_MAYORES_MULTIPLE(P_OPE    NUMBER,
                                    P_TC     NUMBER,
                                    P_MONTO  NUMBER,
                                    P_FECINI VARCHAR2,
                                    P_FECFIN VARCHAR2,
                                    C_TABLE  OUT MYCURSOR) IS
  BEGIN
    OPEN C_TABLE FOR

      SELECT

       TO_CHAR(ROWNUM, '00000000') AS FILA, --1-vb.801
       '001 ' AS OFICINA, --2
       TO_CHAR(ROWNUM, '00000000') AS OPERACION, --3-vb.802
       RPAD(TRIM(TRIM(TO_CHAR(K.DBILLDATE, 'YYYYMM')) || TRIM(TO_CHAR(DECODE(TRIM(K.NPOLICY), '', 0, K.NPOLICY), '0000')) || TRIM(TO_CHAR(TRIM(K.NCERTIF), '0000000000'))), 12) AS INTERNO, --4
       'M' AS MODALIDAD, --5-vb.827
       '150141' AS OPE_UBIGEO, --6
       RPAD(DECODE(K.DBILLDATE, '', ' ', TO_CHAR(K.DBILLDATE, 'YYYYMMDD')), 8) AS OPE_FECHA, --7
       RPAD(DECODE(K.DCOMPDATE, '', ' ', TO_CHAR(K.DCOMPDATE, 'HH24MISS')), 6) AS OPE_HORA, --8
       RPAD(' ', 10) AS EJE_RELACION, --vb.835
       RPAD(' ', 1) AS EJE_CONDICION, --vb.836
       RPAD(' ', 1) AS EJE_TIPPER, --vb
       RPAD(' ', 1) AS EJE_TIPDOC, --vb
       RPAD(' ', 12) AS EJE_NUMDOC, --vb
       RPAD(' ', 11) AS EJE_NUMRUC, --vb
       RPAD(' ', 40) AS EJE_APEPAT, --vb
       RPAD(' ', 40) AS EJE_APEMAT, --vb
       RPAD(' ', 40) AS EJE_NOMBRES, --vb
       RPAD(' ', 4) AS EJE_OCUPACION, --vb
       --'' AS EJE_CIIU, --vb.845
       RPAD(' ', 6) AS EJE_PAIS,
       --'' AS EJE_DESCIIU, --vb.846
       RPAD(' ', 104) AS EJE_CARGO, --vb.847
       RPAD(' ', 2) AS EJE_PEP,
       RPAD(' ', 150) AS EJE_DOMICILIO, --vb.848
       RPAD(' ', 2) AS EJE_DEPART, --vb.849
       RPAD(' ', 2) AS EJE_PROV, --vb.850
       RPAD(' ', 2) AS EJE_DIST, --vb.851
       RPAD(' ', 40) AS EJE_TELEFONO, --vb.852

       RPAD('1', 1) AS ORD_RELACION, --27-vb.735
       RPAD('1', 1) AS ORD_CONDICION, --28-vb.736
       RPAD('1', 1) AS ORD_TIPPER, --29-
       RPAD(DECODE(SUBSTR(CLASEG.SCLIENT, 1, 2), '02', '1', '01', '9', ' '), 1) AS ORD_TIPDOC, --30-vb.738
       RPAD(DECODE(SUBSTR(CLASEG.SCLIENT, 1, 2), '02', RIGHT(CLASEG.SCLIENT, 8), '01', ' ', RIGHT(CLASEG.SCLIENT, 12)), 12) AS ORD_NUMDOC, --31-vb.671
       RPAD(DECODE(SUBSTR(CLASEG.SCLIENT, 1, 2), '02', CLASEG.STAX_CODE, '01', RIGHT(CLASEG.SCLIENT, 11), ' '), 11) AS ORD_NUMRUC, --32
       RPAD(REPLACE(REPLACE(REPLACE(DECODE(SUBSTR(CLASEG.SCLIENT, 1, 2), '02', CLASEG.SLASTNAME, '01', CLASEG.SCLIENAME, ' '), '?', '#'), 'Ñ', '#'), 'ñ', '#'), 120) AS ORD_APEPAT, --33

       RPAD(REPLACE(REPLACE(REPLACE(DECODE(CLASEG.SLASTNAME2, '', ' ', CLASEG.SLASTNAME2), '?', '#'), 'Ñ', '#'), 'ñ', '#'), 40) AS ORD_APEMAT, --34
       RPAD(REPLACE(REPLACE(REPLACE(DECODE(CLASEG.SFIRSTNAME, '', ' ', CLASEG.SFIRSTNAME), '?', '#'), 'Ñ', '#'), 'ñ', '#'), 40) AS ORD_NOMBRES, --35

       RPAD(DECODE(CLASEG.COD_OCUPACION_ASEGU, '', ' ', CLASEG.COD_OCUPACION_ASEGU), 4) AS ORD_OCUPACION, --36-vb.744
       RPAD('PE', 6) AS ORD_PAIS, --37
       --RPAD('' AS Ord_CIIU,--37-YA NO SE SOLICITA
       --RPAD('' AS Ord_DesCIIU,--38-YA NO SE SOLICITA
       RPAD(' ', 104) AS ORD_CARGO, --38-NO ES OBLIGATORIO.vb747
       RPAD(' ', 2) AS ORD_PEP, --39-NO ES OBLIGATORIO
       CLASEG.SSTREET AS ORD_DIRECCION,
       RPAD(REPLACE(REPLACE(REPLACE(DECODE(CLASEG.SSTREET, '', ' ', TRIM(CLASEG.SSTREET)) || ' ' || TRIM(CLASEG.DISTRITO) || ' ' || TRIM(CLASEG.PROV) || ' ' || TRIM(CLASEG.DEPART), '?', '#'),
                            'Ñ',
                            '#'),
                    'ñ',
                    '#'),
            150) AS ORD_DOMICILIO, --40
       TO_NUMBER(CLASEG.COD_UBI_CLI) AS UBIGEO,
       RPAD(DECODE(CLASEG.NMUNICIPALITY, '', '15', SUBSTR(TRIM(TO_CHAR((CLASEG.COD_UBI_CLI), '000000')), 1, 2)), 2) AS ORD_DEPART, --41
       RPAD(DECODE(CLASEG.NMUNICIPALITY, '', '01', SUBSTR(TRIM(TO_CHAR((CLASEG.COD_UBI_CLI), '000000')), 3, 2)), 2) AS ORD_PROV, --42
       RPAD(DECODE(CLASEG.NMUNICIPALITY, '', '41', SUBSTR(TRIM(TO_CHAR((CLASEG.COD_UBI_CLI), '000000')), 5, 2)), 2) AS ORD_DIST, --43
       RPAD(DECODE(CLASEG.SPHONE, '', ' ', CLASEG.SPHONE), 40) AS ORD_TELEFONO, --44
       RPAD('1', 1) AS BEN_RELACION, --45
       RPAD('1', 1) AS BEN_CONDICION, --46
       RPAD(DECODE(SUBSTR(CLBENE.SCLIENT, 1, 2), '02', '2', '01', '3', ' '), 1) AS BEN_TIP_PER, --47
       RPAD(DECODE(SUBSTR(CLBENE.SCLIENT, 1, 2), '02', '1', '01', '9', ' '), 1) AS BEN_TIP_DOC, --48
       RPAD(DECODE(SUBSTR(CLBENE.SCLIENT, 1, 2), '02', RIGHT(CLBENE.SCLIENT, 8), '01', ' ', RIGHT(CLBENE.SCLIENT, 12)), 12) AS BEN_NUM_DOC, --49
       RPAD(DECODE(SUBSTR(CLBENE.SCLIENT, 1, 2), '02', CLBENE.STAX_CODE, '01', RIGHT(CLBENE.SCLIENT, 11), ' '), 11) AS BEN_NUM_RUC, --50
       RPAD(REPLACE(REPLACE(REPLACE(DECODE(SUBSTR(CLBENE.SCLIENT, 1, 2), '02', CLBENE.SLASTNAME, '01', CLBENE.SCLIENAME, ' '), '?', '#'), 'Ñ', '#'), 'ñ', '#'), 120) AS BEN_APEPAT, --51
       RPAD(REPLACE(REPLACE(REPLACE(DECODE(CLBENE.SLASTNAME2, '', ' ', CLBENE.SLASTNAME2), '?', '#'), 'Ñ', '#'), 'ñ', '#'), 40) AS BEN_APEMAT, --52
       RPAD(REPLACE(REPLACE(REPLACE(DECODE(CLBENE.SFIRSTNAME, '', ' ', CLBENE.SFIRSTNAME), '?', '#'), 'Ñ', '#'), 'ñ', '#'), 40) AS BEN_NOMBRES, --53

       RPAD(DECODE(CLBENE.COD_OCUPACION_ASEGU, '', ' ', CLASEG.COD_OCUPACION_ASEGU), 4) AS BEN_OCUPACION, --54
       --RPAD('' AS Ben_CIIU,--55-YA NO SE SOLICITA
       RPAD('PE', 6) /*tb66.sdescript*/ AS BEN_PAIS, --55
       --RPAD('' AS Ben_Des_CIIU,--56-YA NO SE SOLICITA
       RPAD(' ', 104) AS BEN_CARGO, --56-NO ES OBLIGATORIO
       RPAD(' ', 2) AS BEN_PEP, --57-NO ES OBLIGATORIO
       CLBENE.SSTREET AS DIRECCION_B,
       RPAD(REPLACE(REPLACE(REPLACE(DECODE(CLBENE.SSTREET, '', ' ', TRIM(CLBENE.SSTREET)) || ' ' || TRIM(CLBENE.DISTRITO) || ' ' || TRIM(CLBENE.PROV) || ' ' || TRIM(CLBENE.DEPART), '?', '#'),
                            'Ñ',
                            '#'),
                    'ñ',
                    '#'),
            150) AS BEN_DOMICILIO, --58
       TO_NUMBER(CLBENE.COD_UBI_CLI) AS BEN_UBIGEO,
       RPAD(DECODE(CLBENE.NMUNICIPALITY, '', '15', SUBSTR(TRIM(TO_CHAR((CLBENE.COD_UBI_CLI), '000000')), 1, 2)), 2) AS BEN_DEPART, --59
       RPAD(DECODE(CLBENE.NMUNICIPALITY, '', '01', SUBSTR(TRIM(TO_CHAR((CLBENE.COD_UBI_CLI), '000000')), 3, 2)), 2) AS BEN_PROV, --60
       RPAD(DECODE(CLBENE.NMUNICIPALITY, '', '41', SUBSTR(TRIM(TO_CHAR((CLBENE.COD_UBI_CLI), '000000')), 5, 2)), 2) AS BEN_DIST, --61
       RPAD(DECODE(CLBENE.SPHONE, '', ' ', CLBENE.SPHONE), 40) AS BEN_TELEFONO, --62
       RPAD('2', 1) AS DAT_TIPFON, --63
       RPAD('34', 2) AS DAT_TIPOPE, --64
       RPAD(' ', 40) AS DAT_DESOPE, --65
       RPAD(' ', 80) AS DAT_ORIFON, --66
       RPAD(DECODE(K.NCURRENCY, '1', 'PEN', 'USD'), 3) AS DAT_MONOPE, --67
       RPAD(' ', 3) AS DAT_MONOPE_A, --68
       RPAD(TO_CHAR(CAST((K.NAMOUNT) AS NUMBER(15, 2)), '000000000000000.00'), 29) AS DAT_MTOOPE, --MTO_PENSIONGAR , MTO_PRIUNI 69
       RPAD(' ', 30) AS DAT_MTOOPEA, --MTO_PENSIONGAR , MTO_PRIUNI 70
       RPAD(' ', 5) AS DAT_COD_ENT_INVO, --71--NO OBLIGATORIO
       RPAD('1', 1) AS DAT_COD_TIP_CTAO, --72--NO OBLIGATORIO
       RPAD('002193116232365', 20) AS DAT_COD_CTAO, --73--NO OBLIGATORIO
       RPAD(' ', 150) AS DAT_ENT_FNC_EXTO, --74--NO OBLIGATORIO
       RPAD(' ', 5) AS DAT_COD_ENT_INVB, --75--NO OBLIGATORIO
       RPAD('1', 1) AS DAT_COD_TIP_CTAB, --76--NO OBLIGATORIO
       RPAD(' ', 20) AS DAT_COD_CTAB, --77--NO OBLIGATORIO
       RPAD(' ', 150) AS DAT_ENT_FNC_EXTB, --78--NO OBLIGATORIO
       RPAD('1', 1) AS DAT_ALCANCE, --79
       RPAD(' ', 2) AS DAT_COD_PAISO, --80--ALCANCE 1, ESTE CAMPO VA EN BLANCO
       RPAD(' ', 2) AS DAT_COD_PAISD, --81--ALCANCE 1, ESTE CAMPO VA EN BLANCO
       RPAD('2', 1) AS DAT_INTOPE, --82--NO OBLIGATORIO
       RPAD('2', 1) AS DAT_FORMA, --83
       RPAD(DECODE(K.SDESCRIPT, '', ' ', K.SDESCRIPT), 40) AS DAT_INFORM, --84--NO OBLIGATORIO
       'PRI' AS ORIGEN

        FROM (

              SELECT /*+INDEX(M IDX$$_498B10001) INDEX(T XPKTABLE36)*/

               L.NPRODUCT,
                L.NPOLICY,
                M.SDESCRIPT   AS DESCRIPCIONPRODUCTO,
                B.DBILLDATE,
                L.NCERTIF,
                C.SCLIENT     AS BENEFICIARIO,
                CLI.SCLIENAME,
                B.NAMOUNT,
                B.NCURRENCY,
                P_TC          AS TIPO_CAMBIO,
                --TO_CHAR(SUM(DECODE(B.NCURRENCY, 1, B.NAMOUNT, 0))) AS SOLES,
                --TO_CHAR(SUM(DECODE(B.NCURRENCY, 2, B.NAMOUNT, ROUND(B.NAMOUNT / P_TC, 4)))) AS DOLARES,
                P.DCOMPDATE,
                L.SCLIENT   AS ASEGURADO,
                T.SDESCRIPT
                FROM PREMIUM    P, --B
                      BILLS      B, --C
                      POLICY     PO, --P
                      LIFE       L, --A
                      CERTIFICAT C, --CT
                      TABLE36    T, --T
                      PRODMASTER M,
                      TABLE5564  E,
                      CLIENT     CLI --CL
               WHERE C.SCERTYPE = L.SCERTYPE
                 AND C.NBRANCH = L.NBRANCH
                 AND C.NPRODUCT = L.NPRODUCT
                 AND C.NPOLICY = L.NPOLICY
                 AND C.NCERTIF = L.NCERTIF
                 AND P.NRECEIPT = L.NRECEIPT
                 AND B.NINSUR_AREA = P.NINSUR_AREA
                 AND B.SBILLING = '1'
                 AND B.SBILLTYPE = P.SBILLTYPE
                 AND B.NBILLNUM = P.NBILLNUM
                 AND PO.SCERTYPE = L.SCERTYPE
                 AND PO.NBRANCH = L.NBRANCH
                 AND PO.NPRODUCT = L.NPRODUCT
                 AND PO.NPAYFREQ = T.NPAYFREQ
                 AND PO.NPOLICY = L.NPOLICY
                 AND L.NPRODUCT = M.NPRODUCT
                 AND B.NBILLSTAT = E.NBILLSTAT
                 AND L.SCLIENT = CLI.SCLIENT
                 AND L.SCERTYPE = '2'
                 AND L.NBRANCH = 1
                 AND L.NCERTIF > 0
                 AND PO.NPAYFREQ = 6
                 AND TRUNC(B.DBILLDATE) BETWEEN P_FECINI AND P_FECFIN
                 AND B.NBILLSTAT <> 2
                 AND DECODE(B.NCURRENCY, 2, L.NPREMIUM, ROUND(L.NPREMIUM / P_TC, 4)) >= P_MONTO
                 AND L.NPRODUCT != 1000

                 AND L.SCLIENT IN (SELECT X.SCLIENT
                                     FROM (SELECT /*+INDEX(AA XPKLIFE,XIDXNRECEIPT) INDEX(BB XIE6PREMIUM,IDX1PREMIUM)*/
                                            AA.NPOLICY,
                                            AA.SCLIENT
                                             FROM LIFE    AA,
                                                  PREMIUM BB,
                                                  BILLS   CC,
                                                  POLICY  PP,
                                                  TABLE36 TT

                                            WHERE BB.NRECEIPT = AA.NRECEIPT
                                              AND CC.NINSUR_AREA = BB.NINSUR_AREA
                                              AND CC.SBILLING = '1'
                                              AND CC.SBILLTYPE = BB.SBILLTYPE
                                              AND CC.NBILLNUM = BB.NBILLNUM
                                              AND PP.SCERTYPE = AA.SCERTYPE
                                              AND PP.NBRANCH = AA.NBRANCH
                                              AND PP.NPRODUCT = AA.NPRODUCT
                                              AND PP.NPOLICY = AA.NPOLICY
                                              AND PP.NPAYFREQ = TT.NPAYFREQ
                                              AND AA.SCERTYPE = '2'
                                              AND AA.NBRANCH = 1
                                              AND AA.NCERTIF > 0
                                              AND PP.NPAYFREQ = 6
                                              AND TRUNC(CC.DBILLDATE) BETWEEN P_FECINI AND P_FECFIN
                                              AND CC.NBILLSTAT <> 2
                                              AND DECODE(CC.NCURRENCY, 2, AA.NPREMIUM, ROUND(AA.NPREMIUM / P_TC, 4)) >= P_MONTO
                                              AND AA.NPRODUCT != 1000
                                            GROUP BY AA.NPOLICY,
                                                     AA.SCLIENT
                                            ORDER BY 1,
                                                     2) X
                                    GROUP BY X.SCLIENT
                                   HAVING COUNT(*) > 1)

               GROUP BY L.NPRODUCT,
                         L.NPOLICY,
                         M.SDESCRIPT,
                         B.DBILLDATE,
                         L.NCERTIF,
                         C.SCLIENT,
                         CLI.SCLIENAME,
                         B.NAMOUNT,
                         B.NCURRENCY,
                         P_TC,
                         P.DCOMPDATE,
                         L.SCLIENT,
                         T.SDESCRIPT

              --****** Frecuente
              UNION ALL
              SELECT /*+INDEX(M IDX$$_498B10001) INDEX(T XPKTABLE36)*/
               L.NPRODUCT,
                L.NPOLICY,
                M.SDESCRIPT   AS DESCRIPCIONPRODUCTO,
                B.DBILLDATE,
                L.NCERTIF,
                C.SCLIENT     AS BENEFICIARIO,
                CLI.SCLIENAME,
                B.NAMOUNT,
                B.NCURRENCY,
                P_TC          AS TIPO_CAMBIO,
                B.DCOMPDATE,
                L.SCLIENT     AS ASEGURADO,
                T.SDESCRIPT

                FROM PREMIUM    P, --B
                      BILLS      B, --C
                      POLICY     PO, --P
                      LIFE       L, --A
                      CERTIFICAT C, --CT
                      TABLE36    T, --T
                      PRODMASTER M,
                      TABLE5564  E,
                      CLIENT     CLI --CL

               WHERE C.SCERTYPE = L.SCERTYPE
                 AND C.NBRANCH = L.NBRANCH
                 AND C.NPRODUCT = L.NPRODUCT
                 AND C.NPOLICY = L.NPOLICY
                 AND C.NCERTIF = L.NCERTIF
                 AND P.NRECEIPT = L.NRECEIPT
                 AND PO.NBRANCH = P.NBRANCH
                 AND PO.NPRODUCT = P.NPRODUCT
                 AND PO.NPOLICY = P.NPOLICY
                 AND B.NINSUR_AREA = P.NINSUR_AREA
                 AND B.SBILLING = '1'
                 AND B.SBILLTYPE = P.SBILLTYPE
                 AND B.NBILLNUM = P.NBILLNUM
                 AND PO.SCERTYPE = L.SCERTYPE
                 AND PO.NBRANCH = L.NBRANCH
                 AND PO.NPRODUCT = L.NPRODUCT
                 AND PO.NPOLICY = L.NPOLICY
                 AND PO.NPAYFREQ = T.NPAYFREQ
                 AND L.NPRODUCT = M.NPRODUCT
                 AND B.NBILLSTAT = E.NBILLSTAT
                 AND CLI.SCLIENT = L.SCLIENT
                 AND P.SCLIENT = L.SCLIENT
                 AND PO.SCLIENT = C.SCLIENT
                 AND P.SCLIENT = CLI.SCLIENT
                 AND L.SCLIENT = CLI.SCLIENT
                 AND L.SCERTYPE = '2'
                 AND L.NBRANCH = 1
                 AND L.NCERTIF > 0
                 AND PO.NPAYFREQ <> 6
                 AND TRUNC(B.DBILLDATE) BETWEEN P_FECINI AND P_FECFIN
                 AND B.NBILLSTAT <> 2
                 AND DECODE(B.NCURRENCY, 2, L.NPREMIUM, ROUND(L.NPREMIUM / P_TC, 4)) >= P_MONTO
                 AND L.NPRODUCT != 1000
                 AND L.SCLIENT IN (SELECT X.SCLIENT
                                     FROM (SELECT AA.NPOLICY,
                                                   AA.SCLIENT
                                              FROM POLICY  PP,
                                                   PREMIUM BB,
                                                   BILLS   CC,
                                                   LIFE    AA,
                                                   TABLE36 TT

                                             WHERE

                                             CC.NINSUR_AREA = BB.NINSUR_AREA
                                          AND CC.SBILLING = '1'
                                          AND CC.SBILLTYPE = BB.SBILLTYPE
                                          AND CC.NBILLNUM = BB.NBILLNUM
                                          AND BB.NRECEIPT = AA.NRECEIPT
                                          AND BB.NBRANCH = PP.NBRANCH
                                          AND BB.NPRODUCT = PP.NPRODUCT
                                          AND PP.SCERTYPE = BB.SCERTYPE
                                          AND PP.NBRANCH = BB.NBRANCH
                                          AND PP.NPRODUCT = BB.NPRODUCT
                                          AND PP.NPOLICY = AA.NPOLICY
                                          AND TT.NPAYFREQ = PP.NPAYFREQ
                                          AND AA.SCERTYPE = '2'
                                          AND AA.NBRANCH = 1
                                          AND AA.NCERTIF > 0
                                          AND PP.NPAYFREQ <> 6
                                          AND TRUNC(CC.DBILLDATE) BETWEEN P_FECINI AND P_FECFIN
                                          AND CC.NBILLSTAT <> 2
                                          AND DECODE(CC.NCURRENCY, 2, AA.NPREMIUM, ROUND(AA.NPREMIUM / P_TC, 4)) >= P_MONTO
                                          AND AA.NPRODUCT != 1000
                                             GROUP BY AA.NPOLICY,
                                                      AA.SCLIENT
                                             ORDER BY 1,
                                                      2) X
                                    GROUP BY X.SCLIENT
                                   HAVING COUNT(*) > 1)

               GROUP BY L.NPRODUCT,
                         L.NPOLICY,
                         M.SDESCRIPT,
                         B.DBILLDATE,
                         L.NCERTIF,
                         C.SCLIENT,
                         CLI.SCLIENAME,
                         B.NAMOUNT,
                         B.NCURRENCY,
                         P_TC,
                         B.DCOMPDATE,
                         L.SCLIENT,
                         T.SDESCRIPT

              --**************life_his ************
              UNION ALL
              SELECT A.NPRODUCT,
                      A.NPOLICY,
                      M.SDESCRIPT  AS DESCRIPCIONPRODUCTO,
                      C.DBILLDATE,
                      A.NCERTIF,
                      CT.SCLIENT   AS BENEFICIARIO,
                      CL.SCLIENAME,
                      C.NAMOUNT,
                      C.NCURRENCY,
                      P_TC         AS TIPO_CAMBIO,
                      B.DCOMPDATE,
                      A.SCLIENT    AS ASEGURADO,
                      T.SDESCRIPT

                FROM LIFE_HIS A
               INNER JOIN CERTIFICAT CT
                  ON CT.SCERTYPE = A.SCERTYPE
                 AND CT.NBRANCH = A.NBRANCH
                 AND CT.NPRODUCT = A.NPRODUCT
                 AND CT.NPOLICY = A.NPOLICY
                 AND CT.NCERTIF = A.NCERTIF
               INNER JOIN PREMIUM B
                  ON B.NRECEIPT = A.NRECEIPT
               INNER JOIN BILLS C
                  ON C.NINSUR_AREA = B.NINSUR_AREA
                 AND C.SBILLING = '1'
                 AND C.SBILLTYPE = B.SBILLTYPE
                 AND C.NBILLNUM = B.NBILLNUM
               INNER JOIN POLICY P
                  ON P.SCERTYPE = A.SCERTYPE
                 AND P.NBRANCH = A.NBRANCH
                 AND P.NPRODUCT = A.NPRODUCT
                 AND P.NPOLICY = A.NPOLICY
               INNER JOIN TABLE36 T
                  ON P.NPAYFREQ = T.NPAYFREQ
               INNER JOIN PRODMASTER M
                  ON A.NPRODUCT = M.NPRODUCT
               INNER JOIN TABLE5564 E
                  ON C.NBILLSTAT = E.NBILLSTAT
               INNER JOIN CLIENT CL
                  ON A.SCLIENT = CL.SCLIENT
               WHERE A.SCERTYPE = '2'
                 AND A.NBRANCH = 1
                 AND A.NCERTIF > 0
                 AND P.NPAYFREQ = 6
                 AND TRUNC(C.DBILLDATE) BETWEEN P_FECINI AND P_FECFIN
                 AND C.NBILLSTAT <> 2
                 AND DECODE(C.NCURRENCY, 2, A.NPREMIUM, ROUND(A.NPREMIUM / P_TC, 4)) >= P_MONTO
                 AND A.NPRODUCT != 1000
                 AND A.SCLIENT IN (SELECT X.SCLIENT
                                     FROM (SELECT A.NPOLICY,
                                                  A.SCLIENT
                                             FROM LIFE_HIS A
                                            INNER JOIN PREMIUM B
                                               ON B.NRECEIPT = A.NRECEIPT
                                            INNER JOIN BILLS C
                                               ON C.NINSUR_AREA = B.NINSUR_AREA
                                              AND C.SBILLING = '1'
                                              AND C.SBILLTYPE = B.SBILLTYPE
                                              AND C.NBILLNUM = B.NBILLNUM
                                            INNER JOIN POLICY P
                                               ON P.SCERTYPE = A.SCERTYPE
                                              AND P.NBRANCH = A.NBRANCH
                                              AND P.NPRODUCT = A.NPRODUCT
                                              AND P.NPOLICY = A.NPOLICY
                                            INNER JOIN TABLE36 T
                                               ON P.NPAYFREQ = T.NPAYFREQ
                                            WHERE A.SCERTYPE = '2'
                                              AND A.NBRANCH = 1
                                              AND A.NCERTIF > 0
                                              AND P.NPAYFREQ = 6
                                              AND TRUNC(C.DBILLDATE) BETWEEN P_FECINI AND P_FECFIN
                                              AND C.NBILLSTAT <> 2
                                              AND DECODE(C.NCURRENCY, 2, A.NPREMIUM, ROUND(A.NPREMIUM / P_TC, 4)) >= P_MONTO
                                              AND A.NPRODUCT != 1000
                                            GROUP BY A.NPOLICY,
                                                     A.SCLIENT
                                            ORDER BY 1,
                                                     2) X
                                    GROUP BY X.SCLIENT
                                   HAVING COUNT(*) > 1)
               GROUP BY A.NPRODUCT,
                         A.NPOLICY,
                         M.SDESCRIPT,
                         C.DBILLDATE,
                         A.NCERTIF,
                         A.SCLIENT,
                         CT.SCLIENT,
                         CL.SCLIENAME,
                         C.NAMOUNT,
                         C.NCURRENCY,
                         P_TC,
                         B.DCOMPDATE,
                         T.SDESCRIPT

              --****** Frecuente
              UNION ALL
              SELECT A.NPRODUCT,
                      A.NPOLICY,
                      M.SDESCRIPT  AS DESCRIPCIONPRODUCTO,
                      C.DBILLDATE,
                      A.NCERTIF,
                      CT.SCLIENT   AS BENEFICIARIO,
                      CL.SCLIENAME,
                      C.NAMOUNT,
                      C.NCURRENCY,
                      P_TC         AS TIPO_CAMBIO,
                      B.DCOMPDATE,
                      A.SCLIENT    AS ASEGURADO,
                      T.SDESCRIPT
                FROM LIFE_HIS A
               INNER JOIN CERTIFICAT CT
                  ON CT.SCERTYPE = A.SCERTYPE
                 AND CT.NBRANCH = A.NBRANCH
                 AND CT.NPRODUCT = A.NPRODUCT
                 AND CT.NPOLICY = A.NPOLICY
                 AND CT.NCERTIF = A.NCERTIF
               INNER JOIN PREMIUM B
                  ON B.NRECEIPT = A.NRECEIPT
               INNER JOIN BILLS C
                  ON C.NINSUR_AREA = B.NINSUR_AREA
                 AND C.SBILLING = '1'
                 AND C.SBILLTYPE = B.SBILLTYPE
                 AND C.NBILLNUM = B.NBILLNUM
               INNER JOIN POLICY P
                  ON P.SCERTYPE = A.SCERTYPE
                 AND P.NBRANCH = A.NBRANCH
                 AND P.NPRODUCT = A.NPRODUCT
                 AND P.NPOLICY = A.NPOLICY
               INNER JOIN TABLE36 T
                  ON P.NPAYFREQ = T.NPAYFREQ
               INNER JOIN PRODMASTER M
                  ON A.NPRODUCT = M.NPRODUCT
               INNER JOIN TABLE5564 E
                  ON C.NBILLSTAT = E.NBILLSTAT
               INNER JOIN CLIENT CL
                  ON A.SCLIENT = CL.SCLIENT
               WHERE A.SCERTYPE = '2'
                 AND A.NBRANCH = 1
                 AND A.NCERTIF > 0
                 AND P.NPAYFREQ <> 6
                 AND TRUNC(C.DBILLDATE) BETWEEN P_FECINI AND P_FECFIN
                 AND C.NBILLSTAT <> 2
                 AND DECODE(C.NCURRENCY, 2, A.NPREMIUM, ROUND(A.NPREMIUM / P_TC, 4)) >= P_MONTO
                 AND A.NPRODUCT != 1000
                 AND A.SCLIENT IN (SELECT X.SCLIENT
                                     FROM (SELECT A.NPOLICY,
                                                  A.SCLIENT
                                             FROM LIFE_HIS A
                                            INNER JOIN PREMIUM B
                                               ON B.NRECEIPT = A.NRECEIPT
                                            INNER JOIN BILLS C
                                               ON C.NINSUR_AREA = B.NINSUR_AREA
                                              AND C.SBILLING = '1'
                                              AND C.SBILLTYPE = B.SBILLTYPE
                                              AND C.NBILLNUM = B.NBILLNUM
                                            INNER JOIN POLICY P
                                               ON P.SCERTYPE = A.SCERTYPE
                                              AND P.NBRANCH = A.NBRANCH
                                              AND P.NPRODUCT = A.NPRODUCT
                                              AND P.NPOLICY = A.NPOLICY
                                            INNER JOIN TABLE36 T
                                               ON P.NPAYFREQ = T.NPAYFREQ
                                            WHERE A.SCERTYPE = '2'
                                              AND A.NBRANCH = 1
                                              AND A.NCERTIF > 0
                                              AND P.NPAYFREQ <> 6
                                              AND TRUNC(C.DBILLDATE) BETWEEN P_FECINI AND P_FECFIN
                                              AND C.NBILLSTAT <> 2
                                              AND DECODE(C.NCURRENCY, 2, A.NPREMIUM, ROUND(A.NPREMIUM / P_TC, 4)) >= P_MONTO
                                              AND A.NPRODUCT != 1000
                                            GROUP BY A.NPOLICY,
                                                     A.SCLIENT
                                            ORDER BY 1,
                                                     2) X
                                    GROUP BY X.SCLIENT
                                   HAVING COUNT(*) > 1)
               GROUP BY A.NPRODUCT,
                         A.NPOLICY,
                         M.SDESCRIPT,
                         C.DBILLDATE,
                         A.NCERTIF,
                         A.SCLIENT,
                         CT.SCLIENT,
                         CL.SCLIENAME,
                         C.NAMOUNT,
                         C.NCURRENCY,
                         P_TC,
                         B.DCOMPDATE,
                         T.SDESCRIPT

              --****** Soat
              UNION ALL
              SELECT CT.NPRODUCT,
                      CT.NPOLICY,
                      M.SDESCRIPT  AS DESCRIPCIONPRODUCTO,
                      C.DBILLDATE,
                      CT.NCERTIF,
                      CT.SCLIENT   AS BENEFICIARIO,
                      CL.SCLIENAME,
                      C.NAMOUNT,
                      C.NCURRENCY,
                      P_TC         AS TIPO_CAMBIO,
                      B.DCOMPDATE,
                      CT.SCLIENT   AS ASEGURADO,
                      T.SDESCRIPT
                FROM CERTIFICAT CT
               INNER JOIN PREMIUM B
                  ON B.SCERTYPE = CT.SCERTYPE
                 AND B.NBRANCH = CT.NBRANCH
                 AND B.NPRODUCT = CT.NPRODUCT
                 AND B.NPOLICY = CT.NPOLICY
               INNER JOIN BILLS C
                  ON C.NINSUR_AREA = B.NINSUR_AREA
                 AND C.SBILLING = '1'
                 AND C.SBILLTYPE = B.SBILLTYPE
                 AND C.NBILLNUM = B.NBILLNUM
               INNER JOIN POLICY P
                  ON P.SCERTYPE = CT.SCERTYPE
                 AND P.NBRANCH = CT.NBRANCH
                 AND P.NPRODUCT = CT.NPRODUCT
                 AND P.NPOLICY = CT.NPOLICY
               INNER JOIN TABLE36 T
                  ON P.NPAYFREQ = T.NPAYFREQ
               INNER JOIN PRODMASTER M
                  ON CT.NPRODUCT = M.NPRODUCT
               INNER JOIN TABLE5564 E
                  ON C.NBILLSTAT = E.NBILLSTAT
               INNER JOIN CLIENT CL
                  ON CT.SCLIENT = CL.SCLIENT
               WHERE CT.SCERTYPE = '2'
                 AND CT.NBRANCH = 1
                 AND CT.NCERTIF > 0
                 AND P.NPAYFREQ = 6
                 AND TRUNC(C.DBILLDATE) BETWEEN P_FECINI AND P_FECFIN
                 AND C.NBILLSTAT <> 2
                 AND DECODE(C.NCURRENCY, 2, B.NPREMIUM, ROUND(B.NPREMIUM / P_TC, 4)) >= P_MONTO
                 AND B.NPRODUCT = 1000
                 AND B.SCLIENT IN (SELECT X.SCLIENT
                                     FROM (SELECT B.NPOLICY,
                                                  B.SCLIENT
                                             FROM PREMIUM B
                                            INNER JOIN BILLS C
                                               ON C.NINSUR_AREA = B.NINSUR_AREA
                                              AND C.SBILLING = '1'
                                              AND C.SBILLTYPE = B.SBILLTYPE
                                              AND C.NBILLNUM = B.NBILLNUM
                                            INNER JOIN POLICY P
                                               ON P.SCERTYPE = B.SCERTYPE
                                              AND P.NBRANCH = B.NBRANCH
                                              AND P.NPRODUCT = B.NPRODUCT
                                              AND P.NPOLICY = B.NPOLICY
                                            INNER JOIN TABLE36 T
                                               ON P.NPAYFREQ = T.NPAYFREQ
                                            WHERE B.SCERTYPE = '2'
                                              AND B.NBRANCH = 1
                                              AND B.NCERTIF > 0
                                              AND P.NPAYFREQ = 6
                                              AND TRUNC(C.DBILLDATE) BETWEEN P_FECINI AND P_FECFIN
                                              AND C.NBILLSTAT <> 2
                                              AND DECODE(C.NCURRENCY, 2, B.NPREMIUM, ROUND(B.NPREMIUM / P_TC, 4)) >= P_MONTO
                                              AND B.NPRODUCT = 1000
                                            GROUP BY B.NPOLICY,
                                                     B.SCLIENT
                                            ORDER BY 1,
                                                     2) X
                                    GROUP BY X.SCLIENT
                                   HAVING COUNT(*) > 1)
               GROUP BY CT.NPRODUCT,
                         CT.NPOLICY,
                         M.SDESCRIPT,
                         C.DBILLDATE,
                         CT.NCERTIF,
                         B.SCLIENT,
                         CT.SCLIENT,
                         CL.SCLIENAME,
                         C.NAMOUNT,
                         C.NCURRENCY,
                         B.DCOMPDATE,
                         T.SDESCRIPT

              ) K

       INNER JOIN (SELECT
                   /*+INDEX(EQUI SYS_C00167813) INDEX(c XPKPROVINCE) INDEX(d XPKTAB_LOCAT) INDEX(e XPKMUNICIPALITY) INDEX(cc CLIENT_COMPLEMENT_PK)*/
                    A.NPERSON_TYP,
                    A.SCLIENAME,
                    A.SFIRSTNAME,
                    A.SLASTNAME,
                    A.SLASTNAME2,
                    A.NQ_CHILD,
                    ADR.SSTREET,
                    C.SDESCRIPT              AS DEPART,
                    D.SDESCRIPT              AS PROV,
                    E.SDESCRIPT              AS DISTRITO,
                    PH.SPHONE,
                    A.STAX_CODE,
                    E.NMUNICIPALITY,
                    CC.CTA_BCRIA_CONTRATANTE,
                    CC.COD_EMP,
                    CC.CIIU_CONTRATANTE,
                    CC.COD_OCUPACION_ASEGU,
                    A.SCLIENT,
                    EQUI.COD_UBI_CLI
                     FROM CLIENT A

                     LEFT OUTER JOIN (SELECT SCLIENT,
                                            NLOCAL,
                                            NPROVINCE,
                                            NMUNICIPALITY,
                                            NCOUNTRY,
                                            SKEYADDRESS,
                                            SSTREET
                                       FROM ADDRESS ADRR
                                      WHERE ADRR.NRECOWNER = 2
                                        AND ADRR.SRECTYPE = 2
                                        AND ADRR.DNULLDATE IS NULL
                                        AND TRIM(ADRR.SKEYADDRESS) || TO_CHAR(ADRR.DCOMPDATE, 'YYYYMMDDHH24MMSS') || TO_CHAR(ADRR.DEFFECDATE, 'YYYYMMDDHH24MMSS') =
                                            (SELECT /*+INDEX (AT XIF2110ADDRESS)*/
                                              MAX(TRIM(AT.SKEYADDRESS) || TO_CHAR(AT.DCOMPDATE, 'YYYYMMDDHH24MMSS') || TO_CHAR(AT.DEFFECDATE, 'YYYYMMDDHH24MMSS'))
                                               FROM ADDRESS AT
                                              WHERE AT.SCLIENT = ADRR.SCLIENT
                                                AND AT.NRECOWNER = 2
                                                AND AT.SRECTYPE = 2
                                                AND AT.DNULLDATE IS NULL)

                                     ) ADR
                       ON ADR.SCLIENT = A.SCLIENT

                     LEFT OUTER JOIN (SELECT
                                     /*+INDEX (PHR IDX_PHONE_1)*/
                                      SPHONE,
                                      NKEYPHONES,
                                      SKEYADDRESS,
                                      DCOMPDATE,
                                      DEFFECDATE
                                       FROM PHONES PHR
                                      WHERE PHR.NRECOWNER = 2
                                        AND PHR.DNULLDATE IS NULL
                                        AND TRIM(PHR.SKEYADDRESS) || PHR.NKEYPHONES || TO_CHAR(PHR.DCOMPDATE, 'YYYYMMDDHH24MMSS') || TO_CHAR(PHR.DEFFECDATE, 'YYYYMMDDHH24MMSS') =
                                            (SELECT /*+INDEX (PHT IDX_PHONE_1)*/
                                              MAX(TRIM(PHT.SKEYADDRESS) || PHT.NKEYPHONES || TO_CHAR(PHT.DCOMPDATE, 'YYYYMMDDHH24MMSS') || TO_CHAR(PHT.DEFFECDATE, 'YYYYMMDDHH24MMSS'))
                                               FROM PHONES PHT
                                              WHERE SUBSTR(PHT.SKEYADDRESS, 2, 14) = SUBSTR(PHR.SKEYADDRESS, 2, 14)
                                                AND PHT.NRECOWNER = 2
                                                AND PHT.DNULLDATE IS NULL)) PH
                       ON SUBSTR(PH.SKEYADDRESS, 2, 14) = A.SCLIENT

                     LEFT JOIN PROVINCE C
                       ON C.NPROVINCE = ADR.NPROVINCE
                     LEFT JOIN TAB_LOCAT D
                       ON D.NLOCAL = ADR.NLOCAL
                     LEFT JOIN MUNICIPALITY E
                       ON E.NMUNICIPALITY = ADR.NMUNICIPALITY
                     LEFT JOIN CLIENT_COMPLEMENT CC
                       ON A.SCLIENT = CC.SCLIENT
                     LEFT OUTER JOIN EQUI_UBIGEO EQUI
                       ON TO_NUMBER(EQUI.COD_UBI_DIS) = E.NMUNICIPALITY
                      AND EQUI.COD_CLI = '11111111111111') CLASEG
          ON CLASEG.SCLIENT = K.ASEGURADO

       INNER JOIN (SELECT
                   /*+INDEX(EQUI SYS_C00167813) INDEX(c XPKPROVINCE) INDEX(d XPKTAB_LOCAT) INDEX(e XPKMUNICIPALITY) INDEX(cc CLIENT_COMPLEMENT_PK)*/
                    A.NPERSON_TYP,
                    A.SCLIENAME,
                    A.SFIRSTNAME,
                    A.SLASTNAME,
                    A.SLASTNAME2,
                    A.NQ_CHILD,
                    ADR.SSTREET,
                    C.SDESCRIPT              AS DEPART,
                    D.SDESCRIPT              AS PROV,
                    E.SDESCRIPT              AS DISTRITO,
                    PH.SPHONE,
                    A.STAX_CODE,
                    E.NMUNICIPALITY,
                    CC.CTA_BCRIA_CONTRATANTE,
                    CC.COD_EMP,
                    CC.CIIU_CONTRATANTE,
                    CC.COD_OCUPACION_ASEGU,
                    A.SCLIENT,
                    EQUI.COD_UBI_CLI
                     FROM CLIENT A

                     LEFT OUTER JOIN (SELECT SCLIENT,
                                            NLOCAL,
                                            NPROVINCE,
                                            NMUNICIPALITY,
                                            NCOUNTRY,
                                            SKEYADDRESS,
                                            SSTREET
                                       FROM ADDRESS ADRR
                                      WHERE ADRR.NRECOWNER = 2
                                        AND ADRR.SRECTYPE = 2
                                        AND ADRR.DNULLDATE IS NULL
                                        AND TRIM(ADRR.SKEYADDRESS) || TO_CHAR(ADRR.DCOMPDATE, 'YYYYMMDDHH24MMSS') || TO_CHAR(ADRR.DEFFECDATE, 'YYYYMMDDHH24MMSS') =
                                            (SELECT /*+INDEX (AT XIF2110ADDRESS)*/
                                              MAX(TRIM(AT.SKEYADDRESS) || TO_CHAR(AT.DCOMPDATE, 'YYYYMMDDHH24MMSS') || TO_CHAR(AT.DEFFECDATE, 'YYYYMMDDHH24MMSS'))
                                               FROM ADDRESS AT
                                              WHERE AT.SCLIENT = ADRR.SCLIENT
                                                AND AT.NRECOWNER = 2
                                                AND AT.SRECTYPE = 2
                                                AND AT.DNULLDATE IS NULL)

                                     ) ADR
                       ON ADR.SCLIENT = A.SCLIENT

                     LEFT OUTER JOIN (SELECT
                                     /*+INDEX (PHR IDX_PHONE_1)*/
                                      SPHONE,
                                      NKEYPHONES,
                                      SKEYADDRESS,
                                      DCOMPDATE,
                                      DEFFECDATE
                                       FROM PHONES PHR
                                      WHERE PHR.NRECOWNER = 2
                                        AND PHR.DNULLDATE IS NULL
                                        AND TRIM(PHR.SKEYADDRESS) || PHR.NKEYPHONES || TO_CHAR(PHR.DCOMPDATE, 'YYYYMMDDHH24MMSS') || TO_CHAR(PHR.DEFFECDATE, 'YYYYMMDDHH24MMSS') =
                                            (SELECT /*+INDEX (PHT IDX_PHONE_1)*/
                                              MAX(TRIM(PHT.SKEYADDRESS) || PHT.NKEYPHONES || TO_CHAR(PHT.DCOMPDATE, 'YYYYMMDDHH24MMSS') || TO_CHAR(PHT.DEFFECDATE, 'YYYYMMDDHH24MMSS'))
                                               FROM PHONES PHT
                                              WHERE SUBSTR(PHT.SKEYADDRESS, 2, 14) = SUBSTR(PHR.SKEYADDRESS, 2, 14)
                                                AND PHT.NRECOWNER = 2
                                                AND PHT.DNULLDATE IS NULL)

                                     ) PH
                       ON SUBSTR(PH.SKEYADDRESS, 2, 14) = A.SCLIENT

                     LEFT JOIN PROVINCE C
                       ON C.NPROVINCE = ADR.NPROVINCE
                     LEFT JOIN TAB_LOCAT D
                       ON D.NLOCAL = ADR.NLOCAL
                     LEFT JOIN MUNICIPALITY E
                       ON E.NMUNICIPALITY = ADR.NMUNICIPALITY
                     LEFT JOIN CLIENT_COMPLEMENT CC
                       ON A.SCLIENT = CC.SCLIENT
                     LEFT OUTER JOIN EQUI_UBIGEO EQUI
                       ON TO_NUMBER(EQUI.COD_UBI_DIS) = E.NMUNICIPALITY
                      AND EQUI.COD_CLI = '11111111111111') CLBENE
          ON CLBENE.SCLIENT = K.BENEFICIARIO;

  END PRIMAS_MAYORES_MULTIPLE;

  PROCEDURE VIDA_MAYORES(P_OPE    NUMBER,
                         P_TC     NUMBER,
                         P_MONTO  NUMBER,
                         P_FECINI VARCHAR2,
                         P_FECFIN VARCHAR2,
                         C_TABLE  OUT MYCURSOR) IS
  BEGIN
    OPEN C_TABLE FOR

      SELECT

       TRIM(TO_CHAR(ROWNUM, '00000000')) AS FILA, --1-vb.801
       '001 ' AS OFICINA, --2
       TRIM(TO_CHAR(ROWNUM, '00000000')) AS OPERACION, --3-vb.802
       RPAD(TRIM(TRIM(TO_CHAR(K.DBILLDATE, 'YYYYMM')) || TRIM(TO_CHAR(DECODE(TRIM(K.NPOLICY), '', 0, K.NPOLICY), '0000')) || TRIM(TO_CHAR(TRIM(K.NCERTIF), '0000000000'))), 20) AS INTERNO, --4
       K.MODALIDAD AS MODALIDAD, --5-vb.827
       '150141' AS OPE_UBIGEO, --6
       RPAD(DECODE(K.DBILLDATE, '', ' ', TO_CHAR(K.DBILLDATE, 'YYYYMMDD')), 8) AS OPE_FECHA, --7
       RPAD(DECODE(K.DCOMPDATE, '', ' ', TO_CHAR(K.DCOMPDATE, 'HH24MISS')), 6) AS OPE_HORA, --8
       RPAD(' ', 1) AS EJE_RELACION, --vb.835
       RPAD(' ', 1) AS EJE_CONDICION, --vb.836
       RPAD(' ', 1) AS EJE_TIPPER, --vb
       RPAD(' ', 1) AS EJE_TIPDOC, --vb
       RPAD(' ', 12) AS EJE_NUMDOC, --vb
       RPAD(' ', 11) AS EJE_NUMRUC, --vb
       RPAD(' ', 40) AS EJE_APEPAT, --vb
       RPAD(' ', 40) AS EJE_APEMAT, --vb
       RPAD(' ', 40) AS EJE_NOMBRES, --vb
       RPAD(' ', 4) AS EJE_OCUPACION, --vb
       --'' AS EJE_CIIU, --vb.845
       RPAD(' ', 6) AS EJE_PAIS,
       --'' AS EJE_DESCIIU, --vb.846
       RPAD(' ', 104) AS EJE_CARGO, --vb.847
       RPAD(' ', 2) AS EJE_PEP,
       RPAD(' ', 150) AS EJE_DOMICILIO, --vb.848
       RPAD(' ', 2) AS EJE_DEPART, --vb.849
       RPAD(' ', 2) AS EJE_PROV, --vb.850
       RPAD(' ', 2) AS EJE_DIST, --vb.851
       RPAD(' ', 40) AS EJE_TELEFONO, --vb.852

       RPAD(DECODE((SELECT MAX(NROLE) FROM ROLES RO
               WHERE RO.SCLIENT = CLASEG.SCLIENT),1,1,2), 1) AS ORD_RELACION, --27-vb.735
       RPAD('1', 1) AS ORD_CONDICION, --28-vb.736
       --RPAD('1', 1) AS ORD_TIPPER, --29-
       RPAD(NVL(CLASEG.TIPO_PERSONA_ASEG, ' '), 1) AS ORD_TIPPER, --29-
       --RPAD(DECODE(SUBSTR(CLASEG.SCLIENT, 1, 2), '02', '1', '01', '9', ' '), 1) AS ORD_TIPDOC, --30-vb.738
       --02/10/2020--RPAD(DECODE(CLASEG.NPERSON_TYP,2,NVL(DECODE(CLASEG.TIP_DOC_ASEG,'',' ',CLASEG.TIP_DOC_ASEG),' ')), 1) AS ORD_TIPDOC,
       RPAD(NVL(CLASEG.TIPO_DOCUMENTO_ASEG, ' '), 1) AS ORD_TIPDOC,
       --RPAD(DECODE(SUBSTR(CLASEG.SCLIENT, 1, 2), '02', RIGHT(CLASEG.SCLIENT, 8), '01', ' ', RIGHT(CLASEG.SCLIENT, 12)), 12) AS ORD_NUMDOC, --31-vb.671
       RPAD(CASE
              WHEN CLASEG.TIPO_PERSONA_ASEG = '1' OR CLASEG.TIPO_PERSONA_ASEG = '2' THEN
               CLASEG.NUM_DOC_ASEG
              ELSE
               ' '
            END,
            12) AS ORD_NUMDOC,
       --RPAD(DECODE(SUBSTR(CLASEG.SCLIENT, 1, 2), '02', CLASEG.STAX_CODE, '01', RIGHT(CLASEG.SCLIENT, 11), ' '), 11) AS ORD_NUMRUC, --32
       --05/10/2020--RPAD(DECODE(CLASEG.TIP_DOC_ASEG,1,DECODE(CLASEG.NUM_DOC_ASEG,'',' ',CLASEG.NUM_DOC_ASEG),' '), 12) AS ORD_NUMRUC,
       RPAD(CASE
              WHEN CLASEG.TIPO_PERSONA_ASEG = '3' OR CLASEG.TIPO_PERSONA_ASEG = '4' THEN
               RIGHT(CLASEG.SCLIENT, 11)
              ELSE
               ' '
            END,
            11) AS ORD_NUMRUC,
       RPAD(REPLACE(REPLACE(REPLACE(CASE
                                      WHEN CLASEG.TIPO_PERSONA_ASEG IN ('1', '2') THEN
                                       NVL(CLASEG.SLASTNAME, ' ')
                                      WHEN CLASEG.TIPO_PERSONA_ASEG IN ('3', '4') THEN
                                       NVL(CLASEG.SCLIENAME, ' ')
                                      ELSE
                                       ' '
                                    END,
                                    '?',
                                    '#'),
                            'Ñ',
                            '#'),
                    'ñ',
                    '#'),
            120) AS ORD_APEPAT, --33

       RPAD(REPLACE(REPLACE(REPLACE(DECODE(CLASEG.SLASTNAME2, '', ' ', CLASEG.SLASTNAME2), '?', '#'), 'Ñ', '#'), 'ñ', '#'), 40) AS ORD_APEMAT, --34
       RPAD(REPLACE(REPLACE(REPLACE(DECODE(CLASEG.SFIRSTNAME, '', ' ', CLASEG.SFIRSTNAME), '?', '#'), 'Ñ', '#'), 'ñ', '#'), 40) AS ORD_NOMBRES, --35

       RPAD(NVL(COD_ESPECIALIDAD_SBS_ASEG, '0999'), 4) AS ORD_OCUPACION, --36-vb.744
       --RPAD('PE', 6) AS ORD_PAIS, --37
       RPAD(DECODE(CLASEG.SCOD_COUNTRY_ISO, '', ' ', CLASEG.SCOD_COUNTRY_ISO), 6) AS ORD_PAIS,
       --RPAD('' AS Ord_CIIU,--37-YA NO SE SOLICITA
       --RPAD('' AS Ord_DesCIIU,--38-YA NO SE SOLICITA
       RPAD(' ', 104) AS ORD_CARGO, --38-NO ES OBLIGATORIO.vb747
       RPAD(' ', 2) AS ORD_PEP, --39-NO ES OBLIGATORIO
       --RPAD(DECODE(CLASEG.SSTREET, '', ' ', CLASEG.SSTREET), 150) AS ORD_DIRECCION,
       RPAD(REPLACE(REPLACE(REPLACE(

       UPPER(DECODE(CLASEG.SSTREET, '', ' ', TRIM(CLASEG.SSTREET))), '?', '#'),'Ñ','#'),'ñ','#'),150) AS ORD_DOMICILIO, --40
       --TO_NUMBER(CLASEG.COD_UBI_CLI) AS UBIGEO,
       RPAD(DECODE(CLASEG.NMUNICIPALITY, '', ' ', SUBSTR(TRIM(TO_CHAR((CLASEG.COD_UBI_CLI), '000000')), 1, 2)), 2) AS ORD_DEPART, --41
       RPAD(DECODE(CLASEG.NMUNICIPALITY, '', ' ', SUBSTR(TRIM(TO_CHAR((CLASEG.COD_UBI_CLI), '000000')), 3, 2)), 2) AS ORD_PROV, --42
       RPAD(DECODE(CLASEG.NMUNICIPALITY, '', ' ', SUBSTR(TRIM(TO_CHAR((CLASEG.COD_UBI_CLI), '000000')), 5, 2)), 2) AS ORD_DIST, --43
       RPAD(DECODE(INSTR(NVL(CLASEG.SPHONE,' '),'.'), 0, CLASEG.SPHONE, ' '), 40) AS ORD_TELEFONO, --44
       RPAD(DECODE((SELECT MAX(NROLE) FROM ROLES RO
               WHERE RO.SCLIENT = CLBENE.SCLIENT),1,1,2), 1) AS BEN_RELACION, --45
       RPAD('1', 1) AS BEN_CONDICION, --46
       --RPAD(DECODE(SUBSTR(CLBENE.SCLIENT, 1, 2), '02', '2', '01', '3', ' '), 1) AS BEN_TIP_PER, --47
       --05/10/2020--RPAD(DECODE(CLBENE.NPERSON_TYP,'',' ',CLBENE.NPERSON_TYP),1) AS BEN_TIP_PER,
       RPAD(NVL(CLBENE.TIPO_PERSONA_BEN, ' '), 1) AS BEN_TIP_PER,
       --05/10/2020--RPAD(DECODE(CLBENE.NPERSON_TYP,2,DECODE(CLBENE.TIP_DOC_BEN,'',' ',CLBENE.TIP_DOC_BEN),' '), 1) AS BEN_TIP_DOC,
       RPAD(NVL(CLBENE.TIPO_DOCUMENTO_BEN, ' '), 1) AS BEN_TIP_DOC,
       --RPAD(DECODE(SUBSTR(CLBENE.SCLIENT, 1, 2), '02', '1', '01', '9', ' '), 1) AS BEN_TIP_DOC, --48
       --RPAD(DECODE(CLBENE.NUM_DOC_BEN,'',' ',CLBENE.NUM_DOC_BEN), 12) AS BEN_NUM_DOC,
       RPAD(CASE
              WHEN CLBENE.TIPO_PERSONA_BEN = '1' OR CLBENE.TIPO_PERSONA_BEN = '2' THEN
               CLBENE.NUM_DOC_BEN
              ELSE
               ' '
            END,
            12) AS BEN_NUM_DOC,
       --RPAD(DECODE(SUBSTR(CLBENE.SCLIENT, 1, 2), '02', RIGHT(CLBENE.SCLIENT, 8), '01', ' ', RIGHT(CLBENE.SCLIENT, 12)), 12) AS BEN_NUM_DOC, --49
       --RPAD(DECODE(SUBSTR(CLBENE.SCLIENT, 1, 2), '02', CLBENE.STAX_CODE, '01', RIGHT(CLBENE.SCLIENT, 11), ' '), 11) AS BEN_NUM_RUC, --50
       RPAD(CASE
              WHEN CLBENE.TIPO_PERSONA_BEN = '3' OR CLBENE.TIPO_PERSONA_BEN = '4' THEN
               RIGHT(CLBENE.SCLIENT, 11)
              ELSE
               ' '
            END,
            11) AS BEN_NUM_RUC,
       RPAD(REPLACE(REPLACE(REPLACE(CASE
                                      WHEN CLBENE.TIPO_PERSONA_BEN = '1' OR CLBENE.TIPO_PERSONA_BEN = '2' THEN
                                       NVL(CLBENE.SLASTNAME, ' ')
                                      WHEN CLBENE.TIPO_PERSONA_BEN = '3' OR CLBENE.TIPO_PERSONA_BEN = '4' THEN
                                       NVL(CLBENE.SCLIENAME, ' ')
                                      ELSE
                                       ' '
                                    END,
                                    '?',
                                    '#'),
                            'Ñ',
                            '#'),
                    'ñ',
                    '#'),
            120) AS BEN_APEPAT,
       --RPAD(REPLACE(REPLACE(REPLACE(DECODE(SUBSTR(CLBENE.SCLIENT, 1, 2), '02', CLBENE.SLASTNAME, '01', CLBENE.SCLIENAME, ' '), '?', '#'), 'Ñ', '#'), 'ñ', '#'), 120) AS BEN_APEPAT, --51
       RPAD(REPLACE(REPLACE(REPLACE(DECODE(CLBENE.SLASTNAME2, '', ' ', CLBENE.SLASTNAME2), '?', '#'), 'Ñ', '#'), 'ñ', '#'), 40) AS BEN_APEMAT, --52
       RPAD(REPLACE(REPLACE(REPLACE(DECODE(CLBENE.SFIRSTNAME, '', ' ', CLBENE.SFIRSTNAME), '?', '#'), 'Ñ', '#'), 'ñ', '#'), 40) AS BEN_NOMBRES, --53

       RPAD(NVL(CLBENE.COD_ESPECIALIDAD_SBS_BEN, '0999'), 4) AS BEN_OCUPACION, --54
       --RPAD('' AS Ben_CIIU,--55-YA NO SE SOLICITA
       --RPAD('PE', 6) /*tb66.sdescript*/ AS BEN_PAIS, --55
       RPAD(NVL(CLBENE.SCOD_COUNTRY_ISO, ' '), 6) AS BEN_PAIS,
       --RPAD('' AS Ben_Des_CIIU,--56-YA NO SE SOLICITA
       RPAD(' ', 104) AS BEN_CARGO, --56-NO ES OBLIGATORIO
       RPAD(' ', 2) AS BEN_PEP, --57-NO ES OBLIGATORIO
       --CLBENE.SSTREET AS DIRECCION_B,
       RPAD(REPLACE(REPLACE(REPLACE(UPPER(DECODE(CLBENE.SSTREET, '', ' ', TRIM(CLBENE.SSTREET))), '?', '#'),
                            'Ñ','#'),'ñ','#'),150) AS BEN_DOMICILIO, --58
       --TO_NUMBER(CLBENE.COD_UBI_CLI) AS BEN_UBIGEO,
       RPAD(DECODE(CLBENE.NMUNICIPALITY, '', ' ', SUBSTR(TRIM(TO_CHAR((CLBENE.COD_UBI_CLI), '000000')), 1, 2)), 2) AS BEN_DEPART, --59
       RPAD(DECODE(CLBENE.NMUNICIPALITY, '', ' ', SUBSTR(TRIM(TO_CHAR((CLBENE.COD_UBI_CLI), '000000')), 3, 2)), 2) AS BEN_PROV, --60
       RPAD(DECODE(CLBENE.NMUNICIPALITY, '', ' ', SUBSTR(TRIM(TO_CHAR((CLBENE.COD_UBI_CLI), '000000')), 5, 2)), 2) AS BEN_DIST, --61
       RPAD(DECODE(INSTR(NVL(CLBENE.SPHONE,' '),'.'), 0, CLBENE.SPHONE, ' '), 40) AS BEN_TELEFONO, --62
       RPAD('2', 1) AS DAT_TIPFON, --63
       RPAD('34', 2) AS DAT_TIPOPE, --64
       RPAD(' ', 40) AS DAT_DESOPE, --65
       RPAD(' ', 80) AS DAT_ORIFON, --66
       RPAD(DECODE(K.NCURRENCY, '1', 'PEN', 'USD'), 3) AS DAT_MONOPE, --67
       RPAD(' ', 3) AS DAT_MONOPE_A, --68
       RPAD(TO_CHAR(CAST((K.NAMOUNT) AS NUMBER(15, 2)), '000000000000000.00'), 29) AS DAT_MTOOPE, --MTO_PENSIONGAR , MTO_PRIUNI 69
       RPAD(' ', 30) AS DAT_MTOOPEA, --MTO_PENSIONGAR , MTO_PRIUNI 70
       RPAD(' ', 5) AS DAT_COD_ENT_INVO, --71--NO OBLIGATORIO
       RPAD('1', 1) AS DAT_COD_TIP_CTAO, --72--NO OBLIGATORIO
       RPAD('002193116232365', 20) AS DAT_COD_CTAO, --73--NO OBLIGATORIO
       RPAD(' ', 150) AS DAT_ENT_FNC_EXTO, --74--NO OBLIGATORIO
       RPAD(' ', 5) AS DAT_COD_ENT_INVB, --75--NO OBLIGATORIO
       RPAD('1', 1) AS DAT_COD_TIP_CTAB, --76--NO OBLIGATORIO
       RPAD(' ', 20) AS DAT_COD_CTAB, --77--NO OBLIGATORIO
       RPAD(' ', 150) AS DAT_ENT_FNC_EXTB, --78--NO OBLIGATORIO
       RPAD('1', 1) AS DAT_ALCANCE, --79
       RPAD(' ', 2) AS DAT_COD_PAISO, --80--ALCANCE 1, ESTE CAMPO VA EN BLANCO
       RPAD(' ', 2) AS DAT_COD_PAISD, --81--ALCANCE 1, ESTE CAMPO VA EN BLANCO
       RPAD('2', 1) AS DAT_INTOPE, --82--NO OBLIGATORIO
       --RPAD('2', 1) AS DAT_FORMA, --83
       RPAD((SELECT DISTINCT DECODE(NIDPAIDTYPE, 1, '9', 2, '3', 3, '3',' ')
             FROM PV_PAYROLL_PAYMENT PPP,
                  PV_PAYROLL_DETAIL  PPD
            WHERE PPD.NIDPAYROLL = PPP.NIDPAYROLL
              AND PPD.NRECEIPT = K.NRECEIPT), 1) AS DAT_FORMA,
       --RPAD(DECODE(K.SDESCRIPT, '', ' ', K.SDESCRIPT), 40) AS DAT_INFORM, --84--NO OBLIGATORIO
       RPAD((SELECT DISTINCT DECODE(NIDPAIDTYPE, 1, 'Otros', 2, 'Medios o plataformas virtual', 3, 'Medios o plataformas virtual',' ')
             FROM PV_PAYROLL_PAYMENT PPP,
                  PV_PAYROLL_DETAIL  PPD
            WHERE PPD.NIDPAYROLL = PPP.NIDPAYROLL
              AND PPD.NRECEIPT = K.NRECEIPT), 40) AS DAT_INFORM,
       'PRI' AS ORIGEN
        FROM (SELECT
              /*+INDEX(P XPKPOLICY) INDEX(CT XPKCERTIFICAT) INDEX(CL XPKCLIENT) INDEX(M XPKPRODMASTER)*/
               A.NPRODUCT,
               A.NPOLICY,
               M.SDESCRIPT AS DESCRIPCIONPRODUCTO,
               C.DBILLDATE,
               A.NCERTIF,
               CT.SCLIENT AS BENEFICIARIO,
               CL.SCLIENAME,
               C.NAMOUNT,
               C.NCURRENCY,
               P_TC AS TIPO_CAMBIO,
               TO_CHAR(DECODE(C.NCURRENCY, 1, C.NAMOUNT, 0)) AS SOLES,
               TO_CHAR(DECODE(C.NCURRENCY, 2, A.NPREMIUM, ROUND(A.NPREMIUM / P_TC, 4))) AS DOLARES,
               B.DCOMPDATE,
               A.SCLIENT AS ASEGURADO,
               T.SDESCRIPT,
               'U' AS MODALIDAD,
               B.NRECEIPT AS NRECEIPT
                FROM LIFE       A,
                     CERTIFICAT CT,
                     PREMIUM    B,
                     BILLS      C,
                     POLICY     P,
                     TABLE36    T,
                     PRODMASTER M,
                     TABLE5564  E,
                     CLIENT     CL
               WHERE CT.SCERTYPE = A.SCERTYPE
                 AND CT.NBRANCH = A.NBRANCH
                 AND CT.NPRODUCT = A.NPRODUCT
                 AND CT.NPOLICY = A.NPOLICY
                 AND CT.NCERTIF = A.NCERTIF
                 AND B.NRECEIPT = A.NRECEIPT
                 AND C.NINSUR_AREA = B.NINSUR_AREA
                 AND C.SBILLING = '1'
                 AND C.SBILLTYPE = B.SBILLTYPE
                 AND C.NBILLNUM = B.NBILLNUM
                 AND P.SCERTYPE = A.SCERTYPE
                 AND P.NBRANCH = A.NBRANCH
                 AND P.NPRODUCT = A.NPRODUCT
                 AND P.NPOLICY = A.NPOLICY
                 AND P.NPAYFREQ = T.NPAYFREQ
                 AND A.NPRODUCT = M.NPRODUCT
                 AND A.NBRANCH = M.NBRANCH
                 AND C.NBILLSTAT = E.NBILLSTAT
                 AND A.SCLIENT = CL.SCLIENT
                 AND A.SCERTYPE = '2'
                 AND A.NBRANCH NOT IN (73,77,66)
                 AND A.NCERTIF > 0
                 AND P.NPAYFREQ = 6
                 AND TRUNC(C.DBILLDATE) BETWEEN P_FECINI AND P_FECFIN
                 AND C.NBILLSTAT <> 2
                 AND DECODE(C.NCURRENCY, 2, A.NPREMIUM, ROUND(A.NPREMIUM / P_TC, 4)) >= P_MONTO
--                 AND NOT B.NPRODUCT IN (117, 120, 1000)

              UNION ALL

              SELECT
              /*+INDEX(P XPKPOLICY) INDEX(CT XPKCERTIFICAT) INDEX(CL XPKCLIENT) INDEX(M XPKPRODMASTER)*/
               A.NPRODUCT,
               A.NPOLICY,
               M.SDESCRIPT AS DESCRIPCIONPRODUCTO,
               C.DBILLDATE,
               A.NCERTIF,
               CT.SCLIENT AS BENEFICIARIO,
               CL.SCLIENAME,
               C.NAMOUNT,
               C.NCURRENCY,
               P_TC AS TIPO_CAMBIO,
               TO_CHAR(DECODE(C.NCURRENCY, 1, C.NAMOUNT, 0)) AS SOLES,
               TO_CHAR(DECODE(C.NCURRENCY, 2, C.NAMOUNT, ROUND(C.NAMOUNT / P_TC, 4))) AS DOLARES,
               B.DCOMPDATE,
               A.SCLIENT AS ASEGURADO,
               T.SDESCRIPT,
               'U' AS MODALIDAD,
               B.NRECEIPT AS NRECEIPT
                FROM LIFE_HIS A
               INNER JOIN CERTIFICAT CT
                  ON CT.SCERTYPE = A.SCERTYPE
                 AND CT.NBRANCH = A.NBRANCH
                 AND CT.NPRODUCT = A.NPRODUCT
                 AND CT.NPOLICY = A.NPOLICY
                 AND CT.NCERTIF = A.NCERTIF
               INNER JOIN PREMIUM B
                  ON B.NRECEIPT = A.NRECEIPT
               INNER JOIN BILLS C
                  ON C.NINSUR_AREA = B.NINSUR_AREA
                 AND C.SBILLING = '1'
                 AND C.SBILLTYPE = B.SBILLTYPE
                 AND C.NBILLNUM = B.NBILLNUM
               INNER JOIN POLICY P
                  ON P.SCERTYPE = A.SCERTYPE
                 AND P.NBRANCH = A.NBRANCH
                 AND P.NPRODUCT = A.NPRODUCT
                 AND P.NPOLICY = A.NPOLICY
               INNER JOIN TABLE36 T
                  ON P.NPAYFREQ = T.NPAYFREQ
               INNER JOIN PRODMASTER M
                  ON A.NPRODUCT = M.NPRODUCT
                  AND A.NBRANCH = M.NBRANCH
               INNER JOIN TABLE5564 E
                  ON C.NBILLSTAT = E.NBILLSTAT
               INNER JOIN CLIENT CL
                  ON A.SCLIENT = CL.SCLIENT
               WHERE A.SCERTYPE = '2'
                 AND A.NBRANCH IN (77,73,66)
                 AND A.NCERTIF > 0
                 AND P.NPAYFREQ = 6
                 AND TRUNC(C.DBILLDATE) BETWEEN P_FECINI AND P_FECFIN
                 AND C.NBILLSTAT <> 2
                 AND DECODE(C.NCURRENCY, 2, C.NAMOUNT, ROUND(C.NAMOUNT / P_TC, 4)) >= P_MONTO
                 --AND NOT B.NPRODUCT IN (117, 120, 1000)

              UNION ALL
              SELECT
              /*+INDEX(P XPKPOLICY) INDEX(CT XPKCERTIFICAT) INDEX(CL XPKCLIENT) INDEX(M XPKPRODMASTER)*/
               A.NPRODUCT,
               A.NPOLICY,
               M.SDESCRIPT AS DESCRIPCIONPRODUCTO,
               C.DBILLDATE,
               A.NCERTIF,
               CT.SCLIENT AS BENEFICIARIO,
               CL.SCLIENAME,
               C.NAMOUNT,
               C.NCURRENCY,
               P_TC AS TIPO_CAMBIO,
               TO_CHAR(DECODE(C.NCURRENCY, 1, C.NAMOUNT, 0)) AS SOLES,
               TO_CHAR(DECODE(C.NCURRENCY, 2, A.NPREMIUM, ROUND(A.NPREMIUM / P_TC, 4))) AS DOLARES,
               B.DCOMPDATE,
               A.SCLIENT AS ASEGURADO,
               T.SDESCRIPT,
               'U' AS MODALIDAD,
               B.NRECEIPT AS NRECEIPT
                FROM LIFE A,
                     CERTIFICAT CT,
                     PREMIUM B,
                     BILLS C,
                     POLICY P,
                     TABLE36 T,
                     TABLE5564 E,
                     PRODMASTER M,
                     CLIENT CL,
                     (SELECT B.SCLIENT,
                             B.NRECEIPT,
                             SUM(DECODE(A.NCURRENCY, 2, A.NAMOUNT, ROUND(A.NAMOUNT / P_TC, 4))) AS MONTO
                        FROM BILLS A
                       INNER JOIN PREMIUM B
                          ON B.NINSUR_AREA = A.NINSUR_AREA
                         AND B.SBILLTYPE = A.SBILLTYPE
                         AND B.NBILLNUM = A.NBILLNUM
                       WHERE B.NBRANCH IN (73,77)--B.NPRODUCT IN (117, 120)
                         AND A.NBILLSTAT <> 2
                         AND TRUNC(A.DBILLDATE) BETWEEN P_FECINI AND P_FECFIN
                       GROUP BY B.SCLIENT,
                                B.NRECEIPT) X

               WHERE CT.SCERTYPE = A.SCERTYPE
                 AND CT.NBRANCH = A.NBRANCH
                 AND CT.NPRODUCT = A.NPRODUCT
                 AND CT.NPOLICY = A.NPOLICY
                 AND CT.NCERTIF = A.NCERTIF
                 AND A.NRECEIPT = B.NRECEIPT
                 AND C.NINSUR_AREA = B.NINSUR_AREA
                 AND C.SBILLING = '1'
                 AND C.SBILLTYPE = B.SBILLTYPE
                 AND C.NBILLNUM = B.NBILLNUM
                 AND P.SCERTYPE = A.SCERTYPE
                 AND P.NBRANCH = A.NBRANCH
                 AND P.NPRODUCT = A.NPRODUCT
                 AND P.NPOLICY = A.NPOLICY
                 AND P.NPAYFREQ = T.NPAYFREQ
                 AND A.NPRODUCT = M.NPRODUCT
                 AND A.NBRANCH = M.NBRANCH
                 AND C.NBILLSTAT = E.NBILLSTAT
                 AND A.SCLIENT = CL.SCLIENT
                 AND X.SCLIENT = CT.SCLIENT
                 AND X.NRECEIPT = B.NRECEIPT
                 AND X.MONTO > P_MONTO
                 AND A.SCERTYPE = '2'
                 --AND A.NBRANCH = 1
                 AND A.NBRANCH IN (73,77)
                 AND A.NCERTIF > 0
                 AND P.NPAYFREQ = 6
                 AND TRUNC(C.DBILLDATE) BETWEEN P_FECINI AND P_FECFIN
                 AND C.NBILLSTAT <> 2

              UNION ALL

              SELECT
              /*+INDEX(P XPKPOLICY) INDEX(CT XPKCERTIFICAT) INDEX(CL XPKCLIENT) INDEX(M XPKPRODMASTER)*/
               CT.NPRODUCT,
               CT.NPOLICY,
               M.SDESCRIPT AS DESCRIPCIONPRODUCTO,
               C.DBILLDATE,
               CT.NCERTIF,
               CT.SCLIENT AS BENEFICIARIO,
               CL.SCLIENAME,
               C.NAMOUNT,
               C.NCURRENCY,
               P_TC AS TIPO_CAMBIO,
               TO_CHAR(DECODE(C.NCURRENCY, 1, C.NAMOUNT, 0)) AS SOLES,
               TO_CHAR(DECODE(C.NCURRENCY, 2, B.NPREMIUM, ROUND(B.NPREMIUM / P_TC, 4))) AS DOLARES,
               B.DCOMPDATE,
               B.SCLIENT AS ASEGURADO,
               T.SDESCRIPT,
               'U' AS MODALIDAD,
               B.NRECEIPT AS NRECEIPT
                FROM CERTIFICAT CT
               INNER JOIN PREMIUM B
                  ON B.SCERTYPE = CT.SCERTYPE
                 AND B.NBRANCH = CT.NBRANCH
                 AND B.NPRODUCT = CT.NPRODUCT
                 AND B.NPOLICY = CT.NPOLICY --and ct.nreceipt=b.nreceipt
               INNER JOIN BILLS C
                  ON C.NINSUR_AREA = B.NINSUR_AREA
                 AND C.SBILLING = '1'
                 AND C.SBILLTYPE = B.SBILLTYPE
                 AND C.NBILLNUM = B.NBILLNUM
               INNER JOIN POLICY P
                  ON P.SCERTYPE = B.SCERTYPE
                 AND P.NBRANCH = B.NBRANCH
                 AND P.NPRODUCT = B.NPRODUCT
                 AND P.NPOLICY = B.NPOLICY
               INNER JOIN TABLE36 T
                  ON P.NPAYFREQ = T.NPAYFREQ
               INNER JOIN PRODMASTER M
                  ON B.NPRODUCT = M.NPRODUCT
                  AND B.NBRANCH = M.NBRANCH
               INNER JOIN TABLE5564 E
                  ON C.NBILLSTAT = E.NBILLSTAT
               INNER JOIN CLIENT CL
                  ON B.SCLIENT = CL.SCLIENT
               WHERE CT.SCERTYPE = '2'
                 AND CT.NBRANCH = 66
                 AND CT.NCERTIF > 0
                 AND P.NPAYFREQ = 6
                 AND TRUNC(C.DBILLDATE) BETWEEN P_FECINI AND P_FECFIN
                 AND C.NBILLSTAT <> 2
                 AND DECODE(C.NCURRENCY, 2, B.NPREMIUM, ROUND(B.NPREMIUM / P_TC, 4)) >= P_MONTO
                 --AND B.NPRODUCT IN (1000)

              -------
              -------
              UNION ALL
              ---
              SELECT /*+INDEX(M IDX$$_498B10001) INDEX(T XPKTABLE36)*/

               L.NPRODUCT,
               L.NPOLICY,
               M.SDESCRIPT   AS DESCRIPCIONPRODUCTO,
               B.DBILLDATE,
               L.NCERTIF,
               C.SCLIENT     AS BENEFICIARIO,
               CLI.SCLIENAME,
               B.NAMOUNT,
               B.NCURRENCY,
               P_TC          AS TIPO_CAMBIO,
               --TO_CHAR(SUM(DECODE(B.NCURRENCY, 1, B.NAMOUNT, 0)))
               '' AS SOLES,
               --TO_CHAR(SUM(DECODE(B.NCURRENCY, 2, B.NAMOUNT, ROUND(B.NAMOUNT / P_TC, 4))))
               '' AS DOLARES,
               P.DCOMPDATE,
               L.SCLIENT AS ASEGURADO,
               T.SDESCRIPT,
               'M' AS MODALIDAD,
               P.NRECEIPT AS NRECEIPT
                FROM PREMIUM    P, --B
                     BILLS      B, --C
                     POLICY     PO, --P
                     LIFE       L, --A
                     CERTIFICAT C, --CT
                     TABLE36    T, --T
                     PRODMASTER M,
                     TABLE5564  E,
                     CLIENT     CLI --CL
               WHERE C.SCERTYPE = L.SCERTYPE
                 AND C.NBRANCH = L.NBRANCH
                 AND C.NPRODUCT = L.NPRODUCT
                 AND C.NPOLICY = L.NPOLICY
                 AND C.NCERTIF = L.NCERTIF
                 AND P.NRECEIPT = L.NRECEIPT
                 AND B.NINSUR_AREA = P.NINSUR_AREA
                 AND B.SBILLING = '1'
                 AND B.SBILLTYPE = P.SBILLTYPE
                 AND B.NBILLNUM = P.NBILLNUM
                 AND PO.SCERTYPE = L.SCERTYPE
                 AND PO.NBRANCH = L.NBRANCH
                 AND PO.NPRODUCT = L.NPRODUCT
                 AND PO.NPAYFREQ = T.NPAYFREQ
                 AND PO.NPOLICY = L.NPOLICY
                 AND L.NPRODUCT = M.NPRODUCT
                 AND L.NBRANCH = M.NBRANCH
                 AND B.NBILLSTAT = E.NBILLSTAT
                 AND L.SCLIENT = CLI.SCLIENT
                 AND L.SCERTYPE = '2'
                 AND L.NBRANCH <> 66
                 AND L.NCERTIF > 0
                 AND PO.NPAYFREQ = 6
                 AND TRUNC(B.DBILLDATE) BETWEEN P_FECINI AND P_FECFIN
                 AND B.NBILLSTAT <> 2
                 AND DECODE(B.NCURRENCY, 2, L.NPREMIUM, ROUND(L.NPREMIUM / P_TC, 4)) >= P_MONTO
                 --AND L.NPRODUCT != 1000

                 AND L.SCLIENT IN (SELECT X.SCLIENT
                                     FROM (SELECT /*+INDEX(AA XPKLIFE,XIDXNRECEIPT) INDEX(BB XIE6PREMIUM,IDX1PREMIUM)*/
                                            AA.NPOLICY,
                                            AA.SCLIENT
                                             FROM LIFE    AA,
                                                  PREMIUM BB,
                                                  BILLS   CC,
                                                  POLICY  PP,
                                                  TABLE36 TT

                                            WHERE BB.NRECEIPT = AA.NRECEIPT
                                              AND CC.NINSUR_AREA = BB.NINSUR_AREA
                                              AND CC.SBILLING = '1'
                                              AND CC.SBILLTYPE = BB.SBILLTYPE
                                              AND CC.NBILLNUM = BB.NBILLNUM
                                              AND PP.SCERTYPE = AA.SCERTYPE
                                              AND PP.NBRANCH = AA.NBRANCH
                                              AND PP.NPRODUCT = AA.NPRODUCT
                                              AND PP.NPOLICY = AA.NPOLICY
                                              AND PP.NPAYFREQ = TT.NPAYFREQ
                                              AND AA.SCERTYPE = '2'
                                              AND AA.NBRANCH <> 66
                                              AND AA.NCERTIF > 0
                                              AND PP.NPAYFREQ = 6
                                              AND TRUNC(CC.DBILLDATE) BETWEEN P_FECINI AND P_FECFIN
                                              AND CC.NBILLSTAT <> 2
                                              AND DECODE(CC.NCURRENCY, 2, AA.NPREMIUM, ROUND(AA.NPREMIUM / P_TC, 4)) >= P_MONTO
                                              --AND AA.NPRODUCT != 1000
                                            GROUP BY AA.NPOLICY,
                                                     AA.SCLIENT
                                            ORDER BY 1,
                                                     2) X
                                    GROUP BY X.SCLIENT
                                   HAVING COUNT(*) > 1)

               GROUP BY L.NPRODUCT,
                        L.NPOLICY,
                        M.SDESCRIPT,
                        B.DBILLDATE,
                        L.NCERTIF,
                        C.SCLIENT,
                        CLI.SCLIENAME,
                        B.NAMOUNT,
                        B.NCURRENCY,
                        P_TC,
                        --SOLES,
                        --DOLARES,
                        P.DCOMPDATE,
                        L.SCLIENT,
                        T.SDESCRIPT,
                        P.NRECEIPT
              --****** Frecuente
              UNION ALL
              SELECT /*+INDEX(M IDX$$_498B10001) INDEX(T XPKTABLE36)*/
               L.NPRODUCT,
               L.NPOLICY,
               M.SDESCRIPT AS DESCRIPCIONPRODUCTO,
               B.DBILLDATE,
               L.NCERTIF,
               C.SCLIENT AS BENEFICIARIO,
               CLI.SCLIENAME,
               B.NAMOUNT,
               B.NCURRENCY,
               P_TC AS TIPO_CAMBIO,
               '' AS SOLES,
               '' AS DOLARES,
               B.DCOMPDATE,
               L.SCLIENT AS ASEGURADO,
               T.SDESCRIPT,
               'M' AS MODALIDAD,
               P.NRECEIPT AS NRECEIPT
                FROM PREMIUM    P, --B
                     BILLS      B, --C
                     POLICY     PO, --P
                     LIFE       L, --A
                     CERTIFICAT C, --CT
                     TABLE36    T, --T
                     PRODMASTER M,
                     TABLE5564  E,
                     CLIENT     CLI --CL

               WHERE C.SCERTYPE = L.SCERTYPE
                 AND C.NBRANCH = L.NBRANCH
                 AND C.NPRODUCT = L.NPRODUCT
                 AND C.NPOLICY = L.NPOLICY
                 AND C.NCERTIF = L.NCERTIF
                 AND P.NRECEIPT = L.NRECEIPT
                 AND PO.NBRANCH = P.NBRANCH
                 AND PO.NPRODUCT = P.NPRODUCT
                 AND PO.NPOLICY = P.NPOLICY
                 AND B.NINSUR_AREA = P.NINSUR_AREA
                 AND B.SBILLING = '1'
                 AND B.SBILLTYPE = P.SBILLTYPE
                 AND B.NBILLNUM = P.NBILLNUM
                 AND PO.SCERTYPE = L.SCERTYPE
                 AND PO.NBRANCH = L.NBRANCH
                 AND PO.NPRODUCT = L.NPRODUCT
                 AND PO.NPOLICY = L.NPOLICY
                 AND PO.NPAYFREQ = T.NPAYFREQ
                 AND L.NPRODUCT = M.NPRODUCT
                 AND L.NBRANCH = M.NBRANCH
                 AND B.NBILLSTAT = E.NBILLSTAT
                 AND CLI.SCLIENT = L.SCLIENT
                 AND P.SCLIENT = L.SCLIENT
                 AND PO.SCLIENT = C.SCLIENT
                 AND P.SCLIENT = CLI.SCLIENT
                 AND L.SCLIENT = CLI.SCLIENT
                 AND L.SCERTYPE = '2'
                 AND L.NBRANCH <> 66
                 AND L.NCERTIF > 0
                 AND PO.NPAYFREQ <> 6
                 AND TRUNC(B.DBILLDATE) BETWEEN P_FECINI AND P_FECFIN
                 AND B.NBILLSTAT <> 2
                 AND DECODE(B.NCURRENCY, 2, L.NPREMIUM, ROUND(L.NPREMIUM / P_TC, 4)) >= P_MONTO
                 --AND L.NPRODUCT != 1000
                 AND L.SCLIENT IN (SELECT X.SCLIENT
                                     FROM (SELECT AA.NPOLICY,
                                                  AA.SCLIENT
                                             FROM POLICY  PP,
                                                  PREMIUM BB,
                                                  BILLS   CC,
                                                  LIFE    AA,
                                                  TABLE36 TT

                                            WHERE CC.NINSUR_AREA = BB.NINSUR_AREA
                                              AND CC.SBILLING = '1'
                                              AND CC.SBILLTYPE = BB.SBILLTYPE
                                              AND CC.NBILLNUM = BB.NBILLNUM
                                              AND BB.NRECEIPT = AA.NRECEIPT
                                              AND BB.NBRANCH = PP.NBRANCH
                                              AND BB.NPRODUCT = PP.NPRODUCT
                                              AND PP.SCERTYPE = BB.SCERTYPE
                                              AND PP.NBRANCH = BB.NBRANCH
                                              AND PP.NPRODUCT = BB.NPRODUCT
                                              AND PP.NPOLICY = AA.NPOLICY
                                              AND TT.NPAYFREQ = PP.NPAYFREQ
                                              AND AA.SCERTYPE = '2'
                                              AND AA.NBRANCH <> 66
                                              AND AA.NCERTIF > 0
                                              AND PP.NPAYFREQ <> 6
                                              AND TRUNC(CC.DBILLDATE) BETWEEN P_FECINI AND P_FECFIN
                                              AND CC.NBILLSTAT <> 2
                                              AND DECODE(CC.NCURRENCY, 2, AA.NPREMIUM, ROUND(AA.NPREMIUM / P_TC, 4)) >= P_MONTO
                                              --AND AA.NPRODUCT != 1000
                                            GROUP BY AA.NPOLICY,
                                                     AA.SCLIENT
                                            ORDER BY 1,
                                                     2) X
                                    GROUP BY X.SCLIENT
                                   HAVING COUNT(*) > 1)

               GROUP BY L.NPRODUCT,
                        L.NPOLICY,
                        M.SDESCRIPT,
                        B.DBILLDATE,
                        L.NCERTIF,
                        C.SCLIENT,
                        CLI.SCLIENAME,
                        B.NAMOUNT,
                        B.NCURRENCY,
                        P_TC,
                        --SOLES,
                        --DOLARES,
                        B.DCOMPDATE,
                        L.SCLIENT,
                        T.SDESCRIPT,
                        P.NRECEIPT
              --**************life_his ************
              UNION ALL
              SELECT A.NPRODUCT,
                     A.NPOLICY,
                     M.SDESCRIPT AS DESCRIPCIONPRODUCTO,
                     C.DBILLDATE,
                     A.NCERTIF,
                     CT.SCLIENT AS BENEFICIARIO,
                     CL.SCLIENAME,
                     C.NAMOUNT,
                     C.NCURRENCY,
                     P_TC AS TIPO_CAMBIO,
                     '' AS SOLES,
                     '' AS DOLARES,
                     B.DCOMPDATE,
                     A.SCLIENT AS ASEGURADO,
                     T.SDESCRIPT,
                     'M' AS MODALIDAD,
                     B.NRECEIPT AS NRECEIPT
                FROM LIFE_HIS A
               INNER JOIN CERTIFICAT CT
                  ON CT.SCERTYPE = A.SCERTYPE
                 AND CT.NBRANCH = A.NBRANCH
                 AND CT.NPRODUCT = A.NPRODUCT
                 AND CT.NPOLICY = A.NPOLICY
                 AND CT.NCERTIF = A.NCERTIF
               INNER JOIN PREMIUM B
                  ON B.NRECEIPT = A.NRECEIPT
               INNER JOIN BILLS C
                  ON C.NINSUR_AREA = B.NINSUR_AREA
                 AND C.SBILLING = '1'
                 AND C.SBILLTYPE = B.SBILLTYPE
                 AND C.NBILLNUM = B.NBILLNUM
               INNER JOIN POLICY P
                  ON P.SCERTYPE = A.SCERTYPE
                 AND P.NBRANCH = A.NBRANCH
                 AND P.NPRODUCT = A.NPRODUCT
                 AND P.NPOLICY = A.NPOLICY
               INNER JOIN TABLE36 T
                  ON P.NPAYFREQ = T.NPAYFREQ
               INNER JOIN PRODMASTER M
                  ON A.NPRODUCT = M.NPRODUCT
                  AND A.NBRANCH = M.NBRANCH
               INNER JOIN TABLE5564 E
                  ON C.NBILLSTAT = E.NBILLSTAT
               INNER JOIN CLIENT CL
                  ON A.SCLIENT = CL.SCLIENT
               WHERE A.SCERTYPE = '2'
                 AND A.NBRANCH <> 66
                 AND A.NCERTIF > 0
                 AND P.NPAYFREQ = 6
                 AND TRUNC(C.DBILLDATE) BETWEEN P_FECINI AND P_FECFIN
                 AND C.NBILLSTAT <> 2
                 AND DECODE(C.NCURRENCY, 2, A.NPREMIUM, ROUND(A.NPREMIUM / P_TC, 4)) >= P_MONTO
                 --AND A.NPRODUCT != 1000
                 AND A.SCLIENT IN (SELECT X.SCLIENT
                                     FROM (SELECT A.NPOLICY,
                                                  A.SCLIENT
                                             FROM LIFE_HIS A
                                            INNER JOIN PREMIUM B
                                               ON B.NRECEIPT = A.NRECEIPT
                                            INNER JOIN BILLS C
                                               ON C.NINSUR_AREA = B.NINSUR_AREA
                                              AND C.SBILLING = '1'
                                              AND C.SBILLTYPE = B.SBILLTYPE
                                              AND C.NBILLNUM = B.NBILLNUM
                                            INNER JOIN POLICY P
                                               ON P.SCERTYPE = A.SCERTYPE
                                              AND P.NBRANCH = A.NBRANCH
                                              AND P.NPRODUCT = A.NPRODUCT
                                              AND P.NPOLICY = A.NPOLICY
                                            INNER JOIN TABLE36 T
                                               ON P.NPAYFREQ = T.NPAYFREQ
                                            WHERE A.SCERTYPE = '2'
                                              AND A.NBRANCH <> 66
                                              AND A.NCERTIF > 0
                                              AND P.NPAYFREQ = 6
                                              AND TRUNC(C.DBILLDATE) BETWEEN P_FECINI AND P_FECFIN
                                              AND C.NBILLSTAT <> 2
                                              AND DECODE(C.NCURRENCY, 2, A.NPREMIUM, ROUND(A.NPREMIUM / P_TC, 4)) >= P_MONTO
                                              --AND A.NPRODUCT != 1000
                                            GROUP BY A.NPOLICY,
                                                     A.SCLIENT
                                            ORDER BY 1,
                                                     2) X
                                    GROUP BY X.SCLIENT
                                   HAVING COUNT(*) > 1)
               GROUP BY A.NPRODUCT,
                        A.NPOLICY,
                        M.SDESCRIPT,
                        C.DBILLDATE,
                        A.NCERTIF,
                        A.SCLIENT,
                        CT.SCLIENT,
                        CL.SCLIENAME,
                        C.NAMOUNT,
                        C.NCURRENCY,
                        P_TC,
                        --SOLES,
                        --DOLARES,
                        B.DCOMPDATE,
                        T.SDESCRIPT,
                        B.NRECEIPT
              --****** Frecuente
              UNION ALL
              SELECT A.NPRODUCT,
                     A.NPOLICY,
                     M.SDESCRIPT AS DESCRIPCIONPRODUCTO,
                     C.DBILLDATE,
                     A.NCERTIF,
                     CT.SCLIENT AS BENEFICIARIO,
                     CL.SCLIENAME,
                     C.NAMOUNT,
                     C.NCURRENCY,
                     P_TC AS TIPO_CAMBIO,
                     '' AS SOLES,
                     '' AS DOLARES,
                     B.DCOMPDATE,
                     A.SCLIENT AS ASEGURADO,
                     T.SDESCRIPT,
                     'M' AS MODALIDAD,
                     B.NRECEIPT AS NRECEIPT
                FROM LIFE_HIS A
               INNER JOIN CERTIFICAT CT
                  ON CT.SCERTYPE = A.SCERTYPE
                 AND CT.NBRANCH = A.NBRANCH
                 AND CT.NPRODUCT = A.NPRODUCT
                 AND CT.NPOLICY = A.NPOLICY
                 AND CT.NCERTIF = A.NCERTIF
               INNER JOIN PREMIUM B
                  ON B.NRECEIPT = A.NRECEIPT
               INNER JOIN BILLS C
                  ON C.NINSUR_AREA = B.NINSUR_AREA
                 AND C.SBILLING = '1'
                 AND C.SBILLTYPE = B.SBILLTYPE
                 AND C.NBILLNUM = B.NBILLNUM
               INNER JOIN POLICY P
                  ON P.SCERTYPE = A.SCERTYPE
                 AND P.NBRANCH = A.NBRANCH
                 AND P.NPRODUCT = A.NPRODUCT
                 AND P.NPOLICY = A.NPOLICY
               INNER JOIN TABLE36 T
                  ON P.NPAYFREQ = T.NPAYFREQ
               INNER JOIN PRODMASTER M
                  ON A.NPRODUCT = M.NPRODUCT
                  AND A.NBRANCH = M.NBRANCH
               INNER JOIN TABLE5564 E
                  ON C.NBILLSTAT = E.NBILLSTAT
               INNER JOIN CLIENT CL
                  ON A.SCLIENT = CL.SCLIENT
               WHERE A.SCERTYPE = '2'
                 AND A.NBRANCH <> 66
                 AND A.NCERTIF > 0
                 AND P.NPAYFREQ <> 6
                 AND TRUNC(C.DBILLDATE) BETWEEN P_FECINI AND P_FECFIN
                 AND C.NBILLSTAT <> 2
                 AND DECODE(C.NCURRENCY, 2, A.NPREMIUM, ROUND(A.NPREMIUM / P_TC, 4)) >= P_MONTO
                 --AND A.NPRODUCT != 1000
                 AND A.SCLIENT IN (SELECT X.SCLIENT
                                     FROM (SELECT A.NPOLICY,
                                                  A.SCLIENT
                                             FROM LIFE_HIS A
                                            INNER JOIN PREMIUM B
                                               ON B.NRECEIPT = A.NRECEIPT
                                            INNER JOIN BILLS C
                                               ON C.NINSUR_AREA = B.NINSUR_AREA
                                              AND C.SBILLING = '1'
                                              AND C.SBILLTYPE = B.SBILLTYPE
                                              AND C.NBILLNUM = B.NBILLNUM
                                            INNER JOIN POLICY P
                                               ON P.SCERTYPE = A.SCERTYPE
                                              AND P.NBRANCH = A.NBRANCH
                                              AND P.NPRODUCT = A.NPRODUCT
                                              AND P.NPOLICY = A.NPOLICY
                                            INNER JOIN TABLE36 T
                                               ON P.NPAYFREQ = T.NPAYFREQ
                                            WHERE A.SCERTYPE = '2'
                                              AND A.NBRANCH <> 66
                                              AND A.NCERTIF > 0
                                              AND P.NPAYFREQ <> 6
                                              AND TRUNC(C.DBILLDATE) BETWEEN P_FECINI AND P_FECFIN
                                              AND C.NBILLSTAT <> 2
                                              AND DECODE(C.NCURRENCY, 2, A.NPREMIUM, ROUND(A.NPREMIUM / P_TC, 4)) >= P_MONTO
                                              --AND A.NPRODUCT != 1000
                                            GROUP BY A.NPOLICY,
                                                     A.SCLIENT
                                            ORDER BY 1,
                                                     2) X
                                    GROUP BY X.SCLIENT
                                   HAVING COUNT(*) > 1)
               GROUP BY A.NPRODUCT,
                        A.NPOLICY,
                        M.SDESCRIPT,
                        C.DBILLDATE,
                        A.NCERTIF,
                        A.SCLIENT,
                        CT.SCLIENT,
                        CL.SCLIENAME,
                        C.NAMOUNT,
                        C.NCURRENCY,
                        P_TC,
                        --SOLES,
                        --DOLARES,
                        B.DCOMPDATE,
                        T.SDESCRIPT,
                        B.NRECEIPT
              --****** Soat
              UNION ALL
              SELECT CT.NPRODUCT,
                     CT.NPOLICY,
                     M.SDESCRIPT AS DESCRIPCIONPRODUCTO,
                     C.DBILLDATE,
                     CT.NCERTIF,
                     CT.SCLIENT AS BENEFICIARIO,
                     CL.SCLIENAME,
                     C.NAMOUNT,
                     C.NCURRENCY,
                     P_TC AS TIPO_CAMBIO,
                     '' AS SOLES,
                     '' AS DOLARES,
                     B.DCOMPDATE,
                     CT.SCLIENT AS ASEGURADO,
                     T.SDESCRIPT,
                     'M' AS MODALIDAD,
                     B.NRECEIPT AS NRECEIPT
                FROM CERTIFICAT CT
               INNER JOIN PREMIUM B
                  ON B.SCERTYPE = CT.SCERTYPE
                 AND B.NBRANCH = CT.NBRANCH
                 AND B.NPRODUCT = CT.NPRODUCT
                 AND B.NPOLICY = CT.NPOLICY
               INNER JOIN BILLS C
                  ON C.NINSUR_AREA = B.NINSUR_AREA
                 AND C.SBILLING = '1'
                 AND C.SBILLTYPE = B.SBILLTYPE
                 AND C.NBILLNUM = B.NBILLNUM
               INNER JOIN POLICY P
                  ON P.SCERTYPE = CT.SCERTYPE
                 AND P.NBRANCH = CT.NBRANCH
                 AND P.NPRODUCT = CT.NPRODUCT
                 AND P.NPOLICY = CT.NPOLICY
               INNER JOIN TABLE36 T
                  ON P.NPAYFREQ = T.NPAYFREQ
               INNER JOIN PRODMASTER M
                  ON CT.NPRODUCT = M.NPRODUCT
                  AND CT.NBRANCH = M.NBRANCH
               INNER JOIN TABLE5564 E
                  ON C.NBILLSTAT = E.NBILLSTAT
               INNER JOIN CLIENT CL
                  ON CT.SCLIENT = CL.SCLIENT
               WHERE CT.SCERTYPE = '2'
                 AND CT.NBRANCH = 66
                 AND CT.NCERTIF > 0
                 AND P.NPAYFREQ = 6
                 AND TRUNC(C.DBILLDATE) BETWEEN P_FECINI AND P_FECFIN
                 AND C.NBILLSTAT <> 2
                 AND DECODE(C.NCURRENCY, 2, B.NPREMIUM, ROUND(B.NPREMIUM / P_TC, 4)) >= P_MONTO
                 --AND B.NPRODUCT = 1000
                 AND B.SCLIENT IN (SELECT X.SCLIENT
                                     FROM (SELECT B.NPOLICY,
                                                  B.SCLIENT
                                             FROM PREMIUM B
                                            INNER JOIN BILLS C
                                               ON C.NINSUR_AREA = B.NINSUR_AREA
                                              AND C.SBILLING = '1'
                                              AND C.SBILLTYPE = B.SBILLTYPE
                                              AND C.NBILLNUM = B.NBILLNUM
                                            INNER JOIN POLICY P
                                               ON P.SCERTYPE = B.SCERTYPE
                                              AND P.NBRANCH = B.NBRANCH
                                              AND P.NPRODUCT = B.NPRODUCT
                                              AND P.NPOLICY = B.NPOLICY
                                            INNER JOIN TABLE36 T
                                               ON P.NPAYFREQ = T.NPAYFREQ
                                            WHERE B.SCERTYPE = '2'
                                              AND B.NBRANCH = 66
                                              AND B.NCERTIF > 0
                                              AND P.NPAYFREQ = 6
                                              AND TRUNC(C.DBILLDATE) BETWEEN P_FECINI AND P_FECFIN
                                              AND C.NBILLSTAT <> 2
                                              AND DECODE(C.NCURRENCY, 2, B.NPREMIUM, ROUND(B.NPREMIUM / P_TC, 4)) >= P_MONTO
                                              --AND B.NPRODUCT = 1000
                                            GROUP BY B.NPOLICY,
                                                     B.SCLIENT
                                            ORDER BY 1,
                                                     2) X
                                    GROUP BY X.SCLIENT
                                   HAVING COUNT(*) > 1)
               GROUP BY CT.NPRODUCT,
                        CT.NPOLICY,
                        M.SDESCRIPT,
                        C.DBILLDATE,
                        CT.NCERTIF,
                        B.SCLIENT,
                        CT.SCLIENT,
                        CL.SCLIENAME,
                        C.NAMOUNT,
                        C.NCURRENCY,
                        --SOLES,
                        --DOLARES,
                        B.DCOMPDATE,
                        T.SDESCRIPT,
                        B.NRECEIPT) K

        
        
        LEFT OUTER JOIN (SELECT
                         /*+INDEX(EQUI SYS_C00167813) INDEX(c XPKPROVINCE) INDEX(d XPKTAB_LOCAT) INDEX(e XPKMUNICIPALITY) INDEX(cc CLIENT_COMPLEMENT_PK)*/
                          A.NPERSON_TYP,
                          A.SCLIENAME,
                          A.SFIRSTNAME,
                          A.SLASTNAME,
                          A.SLASTNAME2,
                          A.NQ_CHILD,
                          INSUDB.PKG_REPORTES_TABLERO_CONTROL.
                            FN_DIRE_CORREO_CLIENTE(P_SCLIENT => A.SCLIENT,
                                                   P_NRECOWNER => 2,
                                                   P_STIPO_DATO => 'D',
                                                   P_SIND_INDIVIDUAL => 'N') AS SSTREET,
                          C.SDESCRIPT AS DEPART,
                          D.SDESCRIPT AS PROV,
                          E.SDESCRIPT AS DISTRITO,
                          PH.SPHONE,
                          A.STAX_CODE,
                          E.NMUNICIPALITY,
                          CC.CTA_BCRIA_CONTRATANTE,
                          CC.COD_EMP,
                          CC.CIIU_CONTRATANTE,
                          CC.COD_OCUPACION_ASEGU,
                          A.SCLIENT,
                          EQUI.COD_UBI_CLI,
                          DCLI.NTYPCLIENTDOC AS TIP_DOC_ASEG,
                          DCLI.SCLINUMDOCU AS NUM_DOC_ASEG,
                          A.NNATIONALITY,
                          NI.SCOD_COUNTRY_ISO,
                          CASE
                            WHEN SUBSTR(A.SCLIENT, 1, 2) = '01' THEN
                             CASE
                               WHEN SUBSTR(A.SCLIENT, 4, 2) = '20' THEN
                                '3' --'Persona juridica'
                               ELSE
                                '1' --'persona natural con negocio'
                             END
                            ELSE
                             '2' --'Persona natural'
                          END AS TIPO_PERSONA_ASEG,
                          CASE
                            WHEN DCLI.NTYPCLIENTDOC = 2 THEN
                             '1'
                            WHEN DCLI.NTYPCLIENTDOC = 4 THEN
                             '2'
                            WHEN DCLI.NTYPCLIENTDOC = 6 THEN
                             '5'
                            WHEN DCLI.NTYPCLIENTDOC = 1 THEN
                             ' '
                            WHEN DCLI.NTYPCLIENTDOC IN (3, 5, 7, 8, 9, 10, 11, 0, 12, 13) THEN
                             '9'
                            ELSE
                             ' '
                          END AS TIPO_DOCUMENTO_ASEG,
                          COS.NIDOCUP_SBS AS COD_ESPECIALIDAD_SBS_ASEG
                           FROM CLIENT A

                           LEFT OUTER JOIN (SELECT SCLIENT,
                                                  NLOCAL,
                                                  NPROVINCE,
                                                  NMUNICIPALITY,
                                                  NCOUNTRY,
                                                  SKEYADDRESS,
                                                  SSTREET
                                             FROM ADDRESS ADRR
                                            WHERE ADRR.NRECOWNER = 2
                                              AND ADRR.SRECTYPE = 2
                                              AND ADRR.DNULLDATE IS NULL
                                              AND TRIM(ADRR.SKEYADDRESS) || TO_CHAR(ADRR.DCOMPDATE, 'YYYYMMDDHH24MMSS') || TO_CHAR(ADRR.DEFFECDATE, 'YYYYMMDDHH24MMSS') =
                                                  (SELECT /*+INDEX (AT XIF2110ADDRESS)*/
                                                    MAX(TRIM(AT.SKEYADDRESS) || TO_CHAR(AT.DCOMPDATE, 'YYYYMMDDHH24MMSS') || TO_CHAR(AT.DEFFECDATE, 'YYYYMMDDHH24MMSS'))
                                                     FROM ADDRESS AT
                                                    WHERE AT.SCLIENT = ADRR.SCLIENT
                                                      AND AT.NRECOWNER = 2
                                                      AND AT.SRECTYPE = 2
                                                      AND AT.DNULLDATE IS NULL)

                                           ) ADR
                             ON ADR.SCLIENT = A.SCLIENT

                           LEFT OUTER JOIN (SELECT
                                           /*+INDEX (PHR IDX_PHONE_1)*/
                                            SPHONE,
                                            NKEYPHONES,
                                            SKEYADDRESS,
                                            DCOMPDATE,
                                            DEFFECDATE
                                             FROM PHONES PHR
                                            WHERE PHR.NRECOWNER = 2
                                              AND PHR.DNULLDATE IS NULL
                                              AND TRIM(PHR.SKEYADDRESS) || PHR.NKEYPHONES || TO_CHAR(PHR.DCOMPDATE, 'YYYYMMDDHH24MMSS') || TO_CHAR(PHR.DEFFECDATE, 'YYYYMMDDHH24MMSS') =
                                                  (SELECT /*+INDEX (PHT IDX_PHONE_1)*/
                                                    MAX(TRIM(PHT.SKEYADDRESS) || PHT.NKEYPHONES || TO_CHAR(PHT.DCOMPDATE, 'YYYYMMDDHH24MMSS') || TO_CHAR(PHT.DEFFECDATE, 'YYYYMMDDHH24MMSS'))
                                                     FROM PHONES PHT
                                                    WHERE SUBSTR(PHT.SKEYADDRESS, 2, 14) = SUBSTR(PHR.SKEYADDRESS, 2, 14)
                                                      AND PHT.NRECOWNER = 2
                                                      AND PHT.DNULLDATE IS NULL)) PH
                             ON SUBSTR(PH.SKEYADDRESS, 2, 14) = A.SCLIENT

                           LEFT OUTER JOIN PROVINCE C
                             ON C.NPROVINCE = ADR.NPROVINCE
                           LEFT OUTER JOIN TAB_LOCAT D
                             ON D.NLOCAL = ADR.NLOCAL
                           LEFT OUTER JOIN MUNICIPALITY E
                             ON E.NMUNICIPALITY = ADR.NMUNICIPALITY
                           LEFT OUTER JOIN CLIENT_COMPLEMENT CC
                             ON A.SCLIENT = CC.SCLIENT
                           LEFT OUTER JOIN CLIDOCUMENTS DCLI
                             ON DCLI.SCLIENT = A.SCLIENT
                           LEFT OUTER JOIN EQUI_UBIGEO EQUI
                             ON TO_NUMBER(EQUI.COD_UBI_DIS) = E.NMUNICIPALITY
                            AND EQUI.COD_CLI = '11111111111111'
                           LEFT OUTER JOIN TBL_TRX_NATIONALITY_ISO NI
                             ON NVL(A.NNATIONALITY, ADR.NCOUNTRY) = NI.NNATIONALITY
                            AND NI.SACTIVE = 1
                           LEFT OUTER JOIN TBL_CONFIG_OCUP_SBS COS
                             ON COS.NIDOCUPACION = A.NSPECIALITY
                            AND COS.SORIGEN_BD = 'TIME'
                            AND COS.SACTIVE = '1'

                         ) CLASEG
          ON CLASEG.SCLIENT = K.ASEGURADO

        LEFT OUTER JOIN (SELECT
                         /*+INDEX(EQUI SYS_C00167813) INDEX(c XPKPROVINCE) INDEX(d XPKTAB_LOCAT) INDEX(e XPKMUNICIPALITY) INDEX(cc CLIENT_COMPLEMENT_PK)*/
                          A.NPERSON_TYP,
                          A.SCLIENAME,
                          A.SFIRSTNAME,
                          A.SLASTNAME,
                          A.SLASTNAME2,
                          A.NQ_CHILD,
                          INSUDB.PKG_REPORTES_TABLERO_CONTROL.
                            FN_DIRE_CORREO_CLIENTE(P_SCLIENT => A.SCLIENT,
                                                   P_NRECOWNER => 2,
                                                   P_STIPO_DATO => 'D',
                                                   P_SIND_INDIVIDUAL => 'N') AS SSTREET,
                          C.SDESCRIPT AS DEPART,
                          D.SDESCRIPT AS PROV,
                          E.SDESCRIPT AS DISTRITO,
                          PH.SPHONE,
                          A.STAX_CODE,
                          E.NMUNICIPALITY,
                          CC.CTA_BCRIA_CONTRATANTE,
                          CC.COD_EMP,
                          CC.CIIU_CONTRATANTE,
                          CC.COD_OCUPACION_ASEGU,
                          A.SCLIENT,
                          EQUI.COD_UBI_CLI,
                          DCLI.NTYPCLIENTDOC AS TIP_DOC_BEN,
                          DCLI.SCLINUMDOCU AS NUM_DOC_BEN,
                          A.NNATIONALITY,
                          NI.SCOD_COUNTRY_ISO,
                          CASE
                            WHEN SUBSTR(A.SCLIENT, 1, 2) = '01' THEN
                             CASE
                               WHEN SUBSTR(A.SCLIENT, 4, 2) = '20' THEN
                                '3' --'Persona juridica'
                               ELSE
                                '1' --'persona natural con negocio'
                             END
                            ELSE
                             '2' --'Persona natural'
                          END AS TIPO_PERSONA_BEN,
                          CASE
                            WHEN DCLI.NTYPCLIENTDOC = 2 THEN
                             '1'
                            WHEN DCLI.NTYPCLIENTDOC = 4 THEN
                             '2'
                            WHEN DCLI.NTYPCLIENTDOC = 6 THEN
                             '5'
                            WHEN DCLI.NTYPCLIENTDOC = 1 THEN
                             ' '
                            WHEN DCLI.NTYPCLIENTDOC = 3 OR DCLI.NTYPCLIENTDOC = 5 OR DCLI.NTYPCLIENTDOC = 7 OR DCLI.NTYPCLIENTDOC = 8 OR DCLI.NTYPCLIENTDOC = 9 OR DCLI.NTYPCLIENTDOC = 10 OR DCLI.NTYPCLIENTDOC = 11 OR
                                 DCLI.NTYPCLIENTDOC = 0 OR DCLI.NTYPCLIENTDOC = 12 OR DCLI.NTYPCLIENTDOC = 13 THEN
                             '9'
                            ELSE
                             ' '
                          END AS TIPO_DOCUMENTO_BEN,
                          COS.NIDOCUP_SBS AS COD_ESPECIALIDAD_SBS_BEN
                           FROM CLIENT A

                           LEFT OUTER JOIN (SELECT SCLIENT,
                                                  NLOCAL,
                                                  NPROVINCE,
                                                  NMUNICIPALITY,
                                                  NCOUNTRY,
                                                  SKEYADDRESS,
                                                  SSTREET
                                             FROM ADDRESS ADRR
                                            WHERE ADRR.NRECOWNER = 2
                                              AND ADRR.SRECTYPE = 2
                                              AND ADRR.DNULLDATE IS NULL
                                              AND TRIM(ADRR.SKEYADDRESS) || TO_CHAR(ADRR.DCOMPDATE, 'YYYYMMDDHH24MMSS') || TO_CHAR(ADRR.DEFFECDATE, 'YYYYMMDDHH24MMSS') =
                                                  (SELECT /*+INDEX (AT XIF2110ADDRESS)*/
                                                    MAX(TRIM(AT.SKEYADDRESS) || TO_CHAR(AT.DCOMPDATE, 'YYYYMMDDHH24MMSS') || TO_CHAR(AT.DEFFECDATE, 'YYYYMMDDHH24MMSS'))
                                                     FROM ADDRESS AT
                                                    WHERE AT.SCLIENT = ADRR.SCLIENT
                                                      AND AT.NRECOWNER = 2
                                                      AND AT.SRECTYPE = 2
                                                      AND AT.DNULLDATE IS NULL)

                                           ) ADR
                             ON ADR.SCLIENT = A.SCLIENT

                           LEFT OUTER JOIN (SELECT
                                           /*+INDEX (PHR IDX_PHONE_1)*/
                                            SPHONE,
                                            NKEYPHONES,
                                            SKEYADDRESS,
                                            DCOMPDATE,
                                            DEFFECDATE
                                             FROM PHONES PHR
                                            WHERE PHR.NRECOWNER = 2
                                              AND PHR.DNULLDATE IS NULL
                                              AND TRIM(PHR.SKEYADDRESS) || PHR.NKEYPHONES || TO_CHAR(PHR.DCOMPDATE, 'YYYYMMDDHH24MMSS') || TO_CHAR(PHR.DEFFECDATE, 'YYYYMMDDHH24MMSS') =
                                                  (SELECT /*+INDEX (PHT IDX_PHONE_1)*/
                                                    MAX(TRIM(PHT.SKEYADDRESS) || PHT.NKEYPHONES || TO_CHAR(PHT.DCOMPDATE, 'YYYYMMDDHH24MMSS') || TO_CHAR(PHT.DEFFECDATE, 'YYYYMMDDHH24MMSS'))
                                                     FROM PHONES PHT
                                                    WHERE SUBSTR(PHT.SKEYADDRESS, 2, 14) = SUBSTR(PHR.SKEYADDRESS, 2, 14)
                                                      AND PHT.NRECOWNER = 2
                                                      AND PHT.DNULLDATE IS NULL)

                                           ) PH
                             ON SUBSTR(PH.SKEYADDRESS, 2, 14) = A.SCLIENT

                           LEFT OUTER JOIN PROVINCE C
                             ON C.NPROVINCE = ADR.NPROVINCE
                           LEFT OUTER JOIN TAB_LOCAT D
                             ON D.NLOCAL = ADR.NLOCAL
                           LEFT OUTER JOIN MUNICIPALITY E
                             ON E.NMUNICIPALITY = ADR.NMUNICIPALITY
                           LEFT OUTER JOIN CLIENT_COMPLEMENT CC
                             ON A.SCLIENT = CC.SCLIENT
                           LEFT OUTER JOIN CLIDOCUMENTS DCLI
                             ON DCLI.SCLIENT = A.SCLIENT
                           LEFT OUTER JOIN EQUI_UBIGEO EQUI
                             ON TO_NUMBER(EQUI.COD_UBI_DIS) = E.NMUNICIPALITY
                            AND EQUI.COD_CLI = '11111111111111'
                           LEFT OUTER JOIN TBL_TRX_NATIONALITY_ISO NI
                             ON NVL(A.NNATIONALITY, ADR.NCOUNTRY) = NI.NNATIONALITY
                            AND NI.SACTIVE = 1
                           LEFT OUTER JOIN TBL_CONFIG_OCUP_SBS COS
                             ON COS.NIDOCUPACION = A.NSPECIALITY
                            AND COS.SORIGEN_BD = 'TIME'
                            AND COS.SACTIVE = '1') CLBENE
          ON CLBENE.SCLIENT = K.BENEFICIARIO;

  END VIDA_MAYORES;





  /*PROCEDURE PAGO_COMISIONES(P_OPE    NUMBER,
                         P_TC     NUMBER,
                         P_MONTO  NUMBER,
                         P_FECINI VARCHAR2,
                         P_FECFIN VARCHAR2,
                         C_TABLE  OUT MYCURSOR) AS

    CURSOR POL_VL IS
      SELECT *
      FROM POLICY PO,PREMIUM PR,BILLS B,TBL_EQUI_BRANCHPRODUCT RA
      WHERE PR.SCERTYPE = PO.SCERTYPE
      AND PR.NBRANCH = PO.NBRANCH
      AND PR.NPRODUCT = PO.NPRODUCT
      AND PR.NPOLICY = PO.NPOLICY
      AND B.NINSUR_AREA = PR.NINSUR_AREA
      AND B.SBILLTYPE = PR.SBILLTYPE
      AND B.NBILLNUM = PR.NBILLNUM
      AND RA.NPRODUCT_OLD = PO.NPRODUCT
      AND RA.NBRANCH IN (73,74,77)
      AND TRUNC(B.DBILLDATE) BETWEEN TRUNC(TO_DATE(P_FECINI, 'DD/MM/YYYY')) AND TRUNC(TO_DATE(P_FECFIN, 'DD/MM/YYYY'));

  BEGIN




  END;*/

  PROCEDURE NC_VIDA_MAYORES(P_OPE    NUMBER,
                            P_TC     NUMBER,
                            P_MONTO  NUMBER,
                            P_FECINI VARCHAR2,
                            P_FECFIN VARCHAR2,
                            C_TABLE  OUT MYCURSOR) IS

    V_SCLIENT_ORD CLIENT.SCLIENT%TYPE;
    V_SPHONE_ORD  PHONES.SPHONE%TYPE;

    V_SCLIENT_AD        CLIENT.SCLIENT%TYPE;
    V_SDIRECCION_ORD    ADDRESS_CLIENT.SDESDIREBUSQ%TYPE;
    V_STI_DIRE          ADDRESS_CLIENT.STI_DIRE%TYPE;
    V_SNOM_DIRECCION    ADDRESS_CLIENT.SNOM_DIRECCION%TYPE;
    V_SNUM_DIRECCION    ADDRESS_CLIENT.SNUM_DIRECCION%TYPE;
    V_STI_BLOCKCHALET   ADDRESS_CLIENT.STI_BLOCKCHALET%TYPE;
    V_SBLOCKCHALET      ADDRESS_CLIENT.SBLOCKCHALET%TYPE;
    V_STI_INTERIOR      ADDRESS_CLIENT.STI_INTERIOR%TYPE;
    V_SNUM_INTERIOR     ADDRESS_CLIENT.SNUM_INTERIOR%TYPE;
    V_STI_CJHT          ADDRESS_CLIENT.STI_CJHT%TYPE;
    V_SNOM_CJHT         ADDRESS_CLIENT.SNOM_CJHT%TYPE;
    V_SETAPA            ADDRESS_CLIENT.SETAPA%TYPE;
    V_SMANZANA          ADDRESS_CLIENT.SMANZANA%TYPE;
    V_SLOTE             ADDRESS_CLIENT.SLOTE%TYPE;
    V_SREFERENCIA       ADDRESS_CLIENT.SREFERENCIA%TYPE;
    V_NMUNICIPALITY_ORD ADDRESS.NMUNICIPALITY%TYPE;
    V_NPROVINCE_ORD     PROVINCE.NPROVINCE%TYPE;
    V_NLOCAL_ORD        TAB_LOCAT.NLOCAL%TYPE;
    V_SCLIENAME_ORD     CLIENT.SCLIENAME%TYPE;
    V_SIDDOC_ORD        CLIENT_IDDOC.SIDDOC%TYPE;
    V_OFICINA           VARCHAR2(4);
    V_OPE_UBIGEO        VARCHAR2(6);
    V_ORD_RELACION      VARCHAR2(1);
    V_ORD_CONDICION     VARCHAR2(1);
    V_ORD_TIPPER        VARCHAR2(1);
    V_ORD_PAIS          VARCHAR2(2);
    V_BEN_RELACION      VARCHAR2(1);
    V_BEN_CONDICION     VARCHAR2(1);
    V_BEN_PAIS          VARCHAR2(2);
    V_DAT_TIPFON        VARCHAR2(1);
    V_DAT_TIPOPE        VARCHAR2(2);
    V_DAT_ALCANCE       VARCHAR2(1);
    V_DAT_FORMA         VARCHAR2(1);
    V_DISTRITO_ORD      MUNICIPALITY.SDESCRIPT%TYPE;
    V_PROVINCIA_ORD     TAB_LOCAT.SDESCRIPT%TYPE;
    V_DEPARTAMENTO_ORD  PROVINCE.SDESCRIPT%TYPE;
    V_ORD_TIPDOC        VARCHAR2(2);

  BEGIN

    BEGIN
      SELECT SVALOR
        INTO V_OFICINA
        FROM LAFT.TBL_CONFIG_REPORTES
       WHERE SORIGEN = 'NC'
         AND SDESCAMPO = 'OFICINA';

      SELECT SVALOR
        INTO V_OPE_UBIGEO
        FROM LAFT.TBL_CONFIG_REPORTES
       WHERE SORIGEN = 'NC'
         AND SDESCAMPO = 'OPE_UBIGEO';

      SELECT SVALOR
        INTO V_ORD_RELACION
        FROM LAFT.TBL_CONFIG_REPORTES
       WHERE SORIGEN = 'NC'
         AND SDESCAMPO = 'ORD_RELACION';

      SELECT SVALOR
        INTO V_ORD_CONDICION
        FROM LAFT.TBL_CONFIG_REPORTES
       WHERE SORIGEN = 'NC'
         AND SDESCAMPO = 'ORD_CONDICION';

      SELECT SVALOR
        INTO V_ORD_TIPPER
        FROM LAFT.TBL_CONFIG_REPORTES
       WHERE SORIGEN = 'NC'
         AND SDESCAMPO = 'ORD_TIPPER';

      SELECT SVALOR
        INTO V_BEN_RELACION
        FROM LAFT.TBL_CONFIG_REPORTES
       WHERE SORIGEN = 'NC'
         AND SDESCAMPO = 'BEN_RELACION';

      SELECT SVALOR
        INTO V_BEN_CONDICION
        FROM LAFT.TBL_CONFIG_REPORTES
       WHERE SORIGEN = 'NC'
         AND SDESCAMPO = 'BEN_CONDICION';

      SELECT SVALOR
        INTO V_DAT_TIPFON
        FROM LAFT.TBL_CONFIG_REPORTES
       WHERE SORIGEN = 'NC'
         AND SDESCAMPO = 'DAT_TIPFON';

      SELECT SVALOR
        INTO V_DAT_TIPOPE
        FROM LAFT.TBL_CONFIG_REPORTES
       WHERE SORIGEN = 'NC'
         AND SDESCAMPO = 'DAT_TIPOPE';

      SELECT SVALOR
        INTO V_DAT_ALCANCE
        FROM LAFT.TBL_CONFIG_REPORTES
       WHERE SORIGEN = 'NC'
         AND SDESCAMPO = 'DAT_ALCANCE';

      SELECT SVALOR
        INTO V_DAT_FORMA
        FROM LAFT.TBL_CONFIG_REPORTES
       WHERE SORIGEN = 'NC'
         AND SDESCAMPO = 'DAT_FORMA';

      /*SELECT SVALOR INTO V_DATA_INTERM FROM LAFT.TBL_CONFIG_REPORTES
      WHERE SORIGEN='SIN' AND SDESCAMPO='DAT_INTOPE';*/

      V_SCLIENT_ORD := '01020517207331';

      SELECT /*+INDEX (AD XIF2110ADDRESS)*/
      --TRIM(CL.SCLIENAME),
      --TRIM(CI.SIDDOC),
       CL.SCLIENAME,
       CI.SIDDOC,
       ADC.STI_DIRE,
       ADC.SNOM_DIRECCION,
       ADC.SNUM_DIRECCION,
       ADC.STI_BLOCKCHALET,
       ADC.SBLOCKCHALET,
       ADC.STI_INTERIOR,
       ADC.SNUM_INTERIOR,
       ADC.STI_CJHT,
       ADC.SNOM_CJHT,
       ADC.SETAPA,
       ADC.SMANZANA,
       ADC.SLOTE,
       ADC.SREFERENCIA,
       NVL(AD.SSTREET, AD.SSTREET1), --TRIM(AD.SSTREET) || TRIM(AD.SSTREET1),
       ADC.SCLIENT,
       AD.NMUNICIPALITY,
       C.SDESCRIPT,
       D.SDESCRIPT,
       E.SDESCRIPT,
       CI.NIDDOC_TYPE,
       ISO.SCOD_COUNTRY_ISO

        INTO V_SCLIENAME_ORD,
             V_SIDDOC_ORD,
             V_STI_DIRE,
             V_SNOM_DIRECCION,
             V_SNUM_DIRECCION,
             V_STI_BLOCKCHALET,
             V_SBLOCKCHALET,
             V_STI_INTERIOR,
             V_SNUM_INTERIOR,
             V_STI_CJHT,
             V_SNOM_CJHT,
             V_SETAPA,
             V_SMANZANA,
             V_SLOTE,
             V_SREFERENCIA,
             V_SDIRECCION_ORD,
             V_SCLIENT_AD,
             V_NMUNICIPALITY_ORD,
             V_DEPARTAMENTO_ORD,
             V_DISTRITO_ORD,
             V_PROVINCIA_ORD,
             V_ORD_TIPDOC,
             V_ORD_PAIS

        FROM ADDRESS AD
        LEFT OUTER JOIN ADDRESS_CLIENT ADC
          ON ADC.SCLIENT = AD.SCLIENT
         AND ADC.NRECOWNER = AD.NRECOWNER
         AND ADC.SKEYADDRESS = AD.SKEYADDRESS
         AND ADC.DEFFECDATE = AD.DEFFECDATE
         AND ADC.SRECTYPE = AD.SRECTYPE
        LEFT OUTER JOIN CLIENT CL
          ON CL.SCLIENT = AD.SCLIENT
        LEFT OUTER JOIN CLIENT_IDDOC CI
          ON CI.SCLIENT = AD.SCLIENT
        LEFT OUTER JOIN PROVINCE C
          ON C.NPROVINCE = AD.NPROVINCE
        LEFT OUTER JOIN TAB_LOCAT D
          ON D.NLOCAL = AD.NLOCAL
        LEFT OUTER JOIN MUNICIPALITY E
          ON E.NMUNICIPALITY = AD.NMUNICIPALITY
        LEFT OUTER JOIN TBL_TRX_NATIONALITY_ISO ISO
          ON (NVL(CL.NNATIONALITY, AD.NCOUNTRY)) = ISO.NNATIONALITY
       WHERE AD.SCLIENT = V_SCLIENT_ORD
         AND AD.NRECOWNER = 2
         AND AD.SRECTYPE = 2
         AND AD.DNULLDATE IS NULL
         AND ISO.SACTIVE = '1'
         AND TRIM(AD.SKEYADDRESS) || TO_CHAR(AD.DCOMPDATE, 'YYYYMMDDHH24MMSS') || TO_CHAR(AD.DEFFECDATE, 'YYYYMMDDHH24MMSS') =
             (SELECT /*+INDEX (ADT XIF2110ADDRESS)*/
               MAX(TRIM(AT.SKEYADDRESS) || TO_CHAR(AT.DCOMPDATE, 'YYYYMMDDHH24MMSS') || TO_CHAR(AT.DEFFECDATE, 'YYYYMMDDHH24MMSS'))
                FROM ADDRESS AT
               WHERE AT.SCLIENT = AD.SCLIENT
                 AND AT.NRECOWNER = 2
                 AND AT.SRECTYPE = 2
                 AND AT.DNULLDATE IS NULL);
    EXCEPTION
      WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE(SQLERRM || CHR(13) || DBMS_UTILITY.FORMAT_ERROR_BACKTRACE || '  ---1');
        NULL;
    END;

    IF V_SCLIENT_AD IS NOT NULL THEN
      PKG_BDU_CLIENTE.SP_FORMA_FORMATDIRE(V_STI_DIRE,
                                          V_SNOM_DIRECCION,
                                          V_SNUM_DIRECCION,
                                          V_STI_BLOCKCHALET,
                                          V_SBLOCKCHALET,
                                          V_STI_INTERIOR,
                                          V_SNUM_INTERIOR,
                                          V_STI_CJHT,
                                          V_SNOM_CJHT,
                                          V_SETAPA,
                                          V_SMANZANA,
                                          V_SLOTE,
                                          V_SREFERENCIA,
                                          V_SDIRECCION_ORD);

    END IF;

    PKG_BDU_CLIENTE.SP_HOMOLDATOSOTROS('FIDELIZACION', 'RUBIGEO', V_NMUNICIPALITY_ORD, V_NMUNICIPALITY_ORD);

    --V_NPROVINCE_ORD := TRIM(TO_CHAR(V_NMUNICIPALITY_ORD, '000000'));
    --V_NLOCAL_ORD    := TRIM(TO_CHAR(V_NMUNICIPALITY_ORD, '000000'));

    BEGIN
      SELECT SPHONE
        INTO V_SPHONE_ORD
        FROM PHONES PH
       WHERE SUBSTR(PH.SKEYADDRESS, 2, 14) = V_SCLIENT_ORD
         AND PH.NRECOWNER = 2
         AND PH.DNULLDATE IS NULL
         AND TRIM(PH.SKEYADDRESS) || PH.NKEYPHONES || TO_CHAR(PH.DCOMPDATE, 'YYYYMMDDHH24MMSS') || TO_CHAR(PH.DEFFECDATE, 'YYYYMMDDHH24MMSS') =
             (SELECT /*+INDEX (PHT IDX_PHONE_1)*/
               MAX(TRIM(PHT.SKEYADDRESS) || PHT.NKEYPHONES || TO_CHAR(PHT.DCOMPDATE, 'YYYYMMDDHH24MMSS') || TO_CHAR(PHT.DEFFECDATE, 'YYYYMMDDHH24MMSS'))
                FROM PHONES PHT
               WHERE SUBSTR(PHT.SKEYADDRESS, 2, 14) = SUBSTR(PH.SKEYADDRESS, 2, 14)
                 AND PHT.NRECOWNER = 2
                 AND PHT.DNULLDATE IS NULL);

    EXCEPTION
      WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE(SQLERRM || CHR(13) || DBMS_UTILITY.FORMAT_ERROR_BACKTRACE || '  ---2');
        NULL;
    END;

    OPEN C_TABLE FOR
      SELECT TRIM(TO_CHAR(ROWNUM, '00000000')) AS FILA, --1-vb.801
             /*'0001'*/
             RPAD(DECODE(V_OFICINA,'',' ',V_OFICINA),4) AS OFICINA, --2
             TRIM(TO_CHAR(ROWNUM, '00000000')) AS OPERACION, --3-vb.802
             RPAD(TRIM(TRIM(TO_CHAR(P.DBILLDATE, 'YYYYMM')) || TRIM(TO_CHAR(DECODE(TRIM(P.NPOLICY), '', 0, P.NPOLICY), '0000')) || TRIM(TO_CHAR(TRIM(P.NCERTIF), '0000000000'))), 20) AS INTERNO, --4
             P.MODALIDAD AS MODALIDAD, --5-vb.827
             /*'150141'*/
             V_OPE_UBIGEO AS OPE_UBIGEO, --6
             RPAD(DECODE(P.DBILLDATE, '', ' ', TO_CHAR(P.DBILLDATE, 'YYYYMMDD')), 8) AS OPE_FECHA, --7
             RPAD(DECODE(P.DCOMPDATE, '', ' ', TO_CHAR(P.DCOMPDATE, 'HH24MISS')), 6) AS OPE_HORA, --8
             RPAD(' ', 10) AS EJE_RELACION, --vb.835
             RPAD(' ', 1) AS EJE_CONDICION, --vb.836
             RPAD(' ', 1) AS EJE_TIPPER, --vb
             RPAD(' ', 1) AS EJE_TIPDOC, --vb
             RPAD(' ', 12) AS EJE_NUMDOC, --vb
             RPAD(' ', 11) AS EJE_NUMRUC, --vb
             RPAD(' ', 40) AS EJE_APEPAT, --vb
             RPAD(' ', 40) AS EJE_APEMAT, --vb
             RPAD(' ', 40) AS EJE_NOMBRES, --vb
             RPAD(' ', 4) AS EJE_OCUPACION, --vb
             --'' AS EJE_CIIU, --vb.845
             RPAD(' ', 6) AS EJE_PAIS,
             --'' AS EJE_DESCIIU, --vb.846
             RPAD(' ', 104) AS EJE_CARGO, --vb.847
             RPAD(' ', 2) AS EJE_PEP,
             RPAD(' ', 150) AS EJE_DOMICILIO, --vb.848
             RPAD(' ', 2) AS EJE_DEPART, --vb.849
             RPAD(' ', 2) AS EJE_PROV, --vb.850
             RPAD(' ', 2) AS EJE_DIST, --vb.851
             RPAD(' ', 40) AS EJE_TELEFONO, --vb.852

             RPAD( /*'1'*/ NVL(V_ORD_RELACION, ' '), 1) AS ORD_RELACION, --27-vb.735
             RPAD( /*'1'*/ NVL(V_ORD_CONDICION, ' '), 1) AS ORD_CONDICION, --28-vb.736
             RPAD(NVL(V_ORD_TIPPER, ' '), 1) AS ORD_TIPPER, --29-

             RPAD(CASE
                    WHEN V_ORD_TIPPER IN ('1', '2') THEN
                     NVL(V_ORD_TIPDOC, ' ')
                    ELSE
                     ' '
                  END

                 ,
                  1) AS ORD_TIPDOC, --30-vb.738
             RPAD(CASE
                    WHEN V_ORD_TIPPER IN ('1', '2') THEN
                     NVL(V_SIDDOC_ORD, ' ')
                    ELSE
                     ' '
                  END,
                  12) AS ORD_NUMDOC, --31-vb.671
             RPAD(CASE
                    WHEN V_ORD_TIPPER IN (3, 4) THEN
                     NVL(V_SIDDOC_ORD, ' ')
                    ELSE
                     ' '
                  END,
                  11) AS ORD_NUMRUC,
             --RPAD(DECODE(DECODE(RIGHT(V_SIDDOC_ORD, 11), '', ' ', RIGHT(V_SIDDOC_ORD, 11)), '', ' '), 11) AS ORD_NUMRUC, --32-vb.672
             RPAD(NVL(V_SCLIENAME_ORD, ' '), 120) AS ORD_APEPAT, --33
             RPAD(' ', 40) AS ORD_APEMAT, --34
             RPAD(' ', 40) AS ORD_NOMBRES, --35
             RPAD(' ', 4) AS ORD_OCUPACION, --36-vb.744
             RPAD(DECODE(V_ORD_PAIS, '', ' ', V_ORD_PAIS), 6) AS ORD_PAIS, --37
             --RPAD('' AS Ord_CIIU,--37-YA NO SE SOLICITA
             --RPAD('' AS Ord_DesCIIU,--38-YA NO SE SOLICITA
             RPAD(' ', 104) AS ORD_CARGO, --38-NO ES OBLIGATORIO.vb747
             RPAD(' ', 2) AS ORD_PEP, --39-NO ES OBLIGATORIO
             RPAD(DECODE(V_SDIRECCION_ORD, '', ' ', TRIM(V_SDIRECCION_ORD)) || ' ' || DECODE(V_DISTRITO_ORD, '', ' ', TRIM(V_DISTRITO_ORD)) || ' ' ||
                  DECODE(V_PROVINCIA_ORD, '', ' ', TRIM(V_PROVINCIA_ORD)) || ' ' || DECODE(V_DEPARTAMENTO_ORD, '', ' ', TRIM(V_DEPARTAMENTO_ORD)),
                  150) AS ORD_DOMICILIO, --40
             RPAD(DECODE(V_NMUNICIPALITY_ORD, '', '15', SUBSTR(V_NMUNICIPALITY_ORD, 1, 2)), 2) AS ORD_DEPART, --41
             RPAD(DECODE(V_NMUNICIPALITY_ORD, '', '01', SUBSTR(V_NMUNICIPALITY_ORD, 3, 2)), 2) AS ORD_PROV, --42
             RPAD(DECODE(V_NMUNICIPALITY_ORD, '', '41', SUBSTR(V_NMUNICIPALITY_ORD, 5, 2)), 2) AS ORD_DIST, --43
             --V_NMUNICIPALITY_ORD AS ORD_UBIGEO,
             
             RPAD(DECODE(INSTR(V_SPHONE_ORD,'.'),0,' ',NVL(TRIM(V_SPHONE_ORD||''), ' ')), 40) AS ORD_TELEFONO, --44

             RPAD('1', 1) AS BEN_RELACION, --45
             RPAD('1', 1) AS BEN_CONDICION, --46
             --RPAD(DECODE(SUBSTR(clBENEF.SCLIENT, 1, 2), '02', '2', '01', '3', ' '), 1) AS BEN_TIP_PER, --47
             RPAD(NVL(CLORDEN.TIPO_PERSONA, ' '), 1) AS BEN_TIP_PER,
             RPAD(NVL(CLORDEN.TIPO_DOCUMENTO, ' '), 1) AS BEN_TIP_DOC,
             RPAD(CASE
                    WHEN CLORDEN.TIPO_PERSONA IN ('1', '2') THEN
                     CLORDEN.NUM_DOC
                    ELSE
                     ' '
                  END,
                  12) AS BEN_NUM_DOC,
             RPAD(CASE
                    WHEN CLORDEN.TIPO_PERSONA IN ('3', '4') THEN
                     RIGHT(CLORDEN.SCLIENT, 11)
                    ELSE
                     ' '
                  END,
                  12) AS BEN_NUM_RUC,
             RPAD(REPLACE(REPLACE(REPLACE(CASE
                                            WHEN CLORDEN.TIPO_PERSONA IN ('1', '2') THEN
                                             NVL(CLORDEN.SLASTNAME, ' ')
                                            WHEN CLORDEN.TIPO_PERSONA IN ('3', '4') THEN
                                             NVL(CLORDEN.SCLIENAME, ' ')
                                            ELSE
                                             ' '
                                          END,
                                          '?',
                                          '#'),
                                  'Ñ',
                                  '#'),
                          'ñ',
                          '#'),
                  120) AS BEN_APEPAT,
             --RPAD(REPLACE(REPLACE(REPLACE(DECODE(SUBSTR(CLORDEN.SCLIENT, 1, 2), '02', CLORDEN.SLASTNAME, '01', CLORDEN.SCLIENAME, ' '), '?', '#'), 'Ñ', '#'), 'ñ', '#'), 120) AS BEN_APEPAT, --51
             RPAD(REPLACE(REPLACE(REPLACE(DECODE(CLORDEN.SLASTNAME2, '', ' ', CLORDEN.SLASTNAME2), '?', '#'), 'Ñ', '#'), 'ñ', '#'), 40) AS BEN_APEMAT, --52
             RPAD(REPLACE(REPLACE(REPLACE(DECODE(CLORDEN.SFIRSTNAME, '', ' ', CLORDEN.SFIRSTNAME), '?', '#'), 'Ñ', '#'), 'ñ', '#'), 40) AS BEN_NOMBRES, --53

             RPAD(NVL(CLORDEN.COD_ESPECIALIDAD_SBS, ' '), 4) AS BEN_OCUPACION, --54
             RPAD(DECODE(CLORDEN.SCOD_COUNTRY_ISO, '', ' ', CLORDEN.SCOD_COUNTRY_ISO), 6) AS BEN_PAIS,
             RPAD(' ', 104) AS BEN_CARGO, --56-NO ES OBLIGATORIO
             RPAD(' ', 2) AS BEN_PEP, --57-NO ES OBLIGATORIO
             RPAD(REPLACE(REPLACE(REPLACE(DECODE(CLORDEN.SSTREET, '', ' ', TRIM(CLORDEN.SSTREET)) || ' ' || TRIM(CLORDEN.DISTRITO) || ' ' || TRIM(CLORDEN.PROV) || ' ' || TRIM(CLORDEN.DEPART),
                                          '?',
                                          '#'),
                                  'Ñ',
                                  '#'),
                          'ñ',
                          '#'),
                  150) AS BEN_DOMICILIO, --58
             --TO_NUMBER(CLORDEN.COD_UBI_CLI) AS BEN_UBIGEO,
             RPAD(DECODE(CLORDEN.NMUNICIPALITY, '', '15', SUBSTR(TRIM(TO_CHAR((CLORDEN.COD_UBI_CLI), '000000')), 1, 2)), 2) AS BEN_DEPART, --59
             RPAD(DECODE(CLORDEN.NMUNICIPALITY, '', '01', SUBSTR(TRIM(TO_CHAR((CLORDEN.COD_UBI_CLI), '000000')), 3, 2)), 2) AS BEN_PROV, --60
             RPAD(DECODE(CLORDEN.NMUNICIPALITY, '', '41', SUBSTR(TRIM(TO_CHAR((CLORDEN.COD_UBI_CLI), '000000')), 5, 2)), 2) AS BEN_DIST, --61
             RPAD(DECODE(INSTR(CLORDEN.SPHONE,'.'),0,' ',NVL(TRIM(CLORDEN.SPHONE||''),' ')), 40) AS BEN_TELEFONO, --62
             RPAD( /*'2'*/ V_DAT_TIPFON, 1) AS DAT_TIPFON, --63
             RPAD( /*'34'*/ V_DAT_TIPOPE, 2) AS DAT_TIPOPE, --64
             RPAD(' ', 40) AS DAT_DESOPE, --65
             RPAD(' ', 80) AS DAT_ORIFON, --66
             RPAD(DECODE(P.NCURRENCY, '1', 'PEN', 'USD'), 3) AS DAT_MONOPE, --67
             RPAD(' ', 3) AS DAT_MONOPE_A, --68
             RPAD(TO_CHAR(CAST((P.NAMOUNT) AS NUMBER(15, 2)), '000000000000000.00'), 29) AS DAT_MTOOPE, --MTO_PENSIONGAR , MTO_PRIUNI 69
             RPAD(' ', 30) AS DAT_MTOOPEA, --MTO_PENSIONGAR , MTO_PRIUNI 70
             RPAD(' ', 5) AS DAT_COD_ENT_INVO, --71--NO OBLIGATORIO
             RPAD('1', 1) AS DAT_COD_TIP_CTAO, --72--NO OBLIGATORIO
             RPAD('002193116232365', 20) AS DAT_COD_CTAO, --73--NO OBLIGATORIO
             RPAD(' ', 150) AS DAT_ENT_FNC_EXTO, --74--NO OBLIGATORIO
             RPAD(' ', 5) AS DAT_COD_ENT_INVB, --75--NO OBLIGATORIO
             RPAD('1', 1) AS DAT_COD_TIP_CTAB, --76--NO OBLIGATORIO
             RPAD(' ', 20) AS DAT_COD_CTAB, --77--NO OBLIGATORIO
             RPAD(' ', 150) AS DAT_ENT_FNC_EXTB, --78--NO OBLIGATORIO
             RPAD( /*'1'*/ V_DAT_ALCANCE, 1) AS DAT_ALCANCE, --79
             RPAD(' ', 2) AS DAT_COD_PAISO, --80--ALCANCE 1, ESTE CAMPO VA EN BLANCO
             RPAD(' ', 2) AS DAT_COD_PAISD, --81--ALCANCE 1, ESTE CAMPO VA EN BLANCO
             RPAD('2', 1) AS DAT_INTOPE, --82--NO OBLIGATORIO
             RPAD( /*'2'*/ V_DAT_FORMA, 1) AS DAT_FORMA, --83
             RPAD(DECODE(P.INFORM, '', ' ', P.INFORM), 40) AS DAT_INFORM --84--NO OBLIGATORIO

        FROM (SELECT DISTINCT PO.NPOLICY,
                              PO.SCLIENT,
                              BI_PRE.NBILLNUM,
                              BI_PRE.NINSUR_AREA,
                              BI_PRE.NRECEIPT,
                              BI_PRE.NPRODUCT,
                              BI_PRE.NBRANCH,
                              BI_PRE.SBILLTYPE,
                              BI_PRE.NPREMIUM,
                              BI_PRE.NCURRENCY,
                              PO.SCLIENT AS ORDENANTE, --RO.SCLIENT AS BENEFICIARIO,
                              BI_PRE.DBILLDATE,
                              BI_PRE.DCOMPDATE,
                              LI.NCERTIF,
                              'U' AS MODALIDAD,
                              BI_PRE.NAMOUNT,
                              T.SDESCRIPT AS INFORM
                FROM (SELECT /*+ INDEX(B IDX$$_498B60002) INDEX(PR IDX1PREMIUM)*/
                      DISTINCT B.NBILLNUM,
                               B.NINSUR_AREA,
                               PR.NPOLICY,
                               PR.NRECEIPT,
                               PR.NPRODUCT,
                               PR.NBRANCH,
                               PR.SCERTYPE,
                               B.SBILLTYPE,
                               PR.NPREMIUM,
                               B.NCURRENCY,
                               B.DBILLDATE,
                               B.DCOMPDATE,
                               B.NAMOUNT
                        FROM BILLS B
                        LEFT JOIN PREMIUM PR
                          ON B.NINSUR_AREA = PR.NINSUR_AREA
                         AND B.SBILLTYPE = PR.SBILLTYPE
                         AND B.NBILLNUM = PR.NBILLNUM
                       WHERE B.SBILLTYPE IN (3, 7, 8)
                         AND DECODE(B.NCURRENCY, 2, PR.NPREMIUM, ROUND(PR.NPREMIUM / P_TC, 4)) >= P_MONTO
                            --AND TO_CHAR(TRUNC(B.DBILLDATE),'DD/MM/YYYY') BETWEEN '01/01/2018' AND '15/01/2018'
                         AND TO_CHAR(TRUNC(B.DBILLDATE), 'DD/MM/YYYY') BETWEEN P_FECINI AND P_FECFIN) BI_PRE
                LEFT JOIN POLICY PO
                  ON PO.SCERTYPE = BI_PRE.SCERTYPE
                 AND PO.NBRANCH = BI_PRE.NBRANCH
                 AND PO.NPRODUCT = BI_PRE.NPRODUCT
                 AND PO.NPOLICY = BI_PRE.NPOLICY
                LEFT JOIN LIFE LI
                  ON LI.NBRANCH = BI_PRE.NBRANCH
                 AND LI.NPRODUCT = BI_PRE.NPRODUCT
                 AND LI.NPOLICY = BI_PRE.NPOLICY
                 AND LI.NRECEIPT = BI_PRE.NRECEIPT
                 AND LI.SCERTYPE = BI_PRE.SCERTYPE
                LEFT JOIN CERTIFICAT CE
                  ON CE.SCERTYPE = LI.SCERTYPE
                 AND CE.NBRANCH = LI.NBRANCH
                 AND CE.NPRODUCT = LI.NPRODUCT
                 AND CE.NPOLICY = LI.NPOLICY
                 AND CE.NCERTIF = LI.NCERTIF
                LEFT OUTER JOIN TABLE36 FP
                  ON FP.NPAYFREQ = CE.NPAYFREQ
                LEFT JOIN PRODMASTER PMA
                  ON PMA.NPRODUCT = PO.NPRODUCT
                LEFT OUTER JOIN TABLE36 T
                  ON T.NPAYFREQ = PO.NPAYFREQ
              /*LEFT JOIN ROLES RO
              ON RO.SCERTYPE = LI.SCERTYPE
              AND RO.NBRANCH = LI.NBRANCH
              AND RO.NPRODUCT = LI.NPRODUCT
              AND RO.NPOLICY = LI.NPOLICY
              AND RO.NCERTIF = LI.NCERTIF
              WHERE RO.NROLE = 2*/
              ) P
      /*LEFT JOIN (
      SELECT C.SCLIENT,ADR.NLOCAL,PROV.SDESCRIPT AS PROV,ADR.NPROVINCE,DPTO.SDESCRIPT AS DEPART,
      ADR.NMUNICIPALITY,DIST.SDESCRIPT AS DISTRITO,ADR.NCOUNTRY,ADR.SSTREET,PH.SPHONE,
      EQUI.COD_UBI_CLI,NI.SCOD_COUNTRY_ISO,CC.CTA_BCRIA_CONTRATANTE,CC.COD_EMP,CC.CIIU_CONTRATANTE,CC.COD_OCUPACION_ASEGU,
      DCLI.NIDDOC_TYPE AS TIP_DOC_BEN,DCLI.SIDDOC AS NUM_DOC_BEN,C.NPERSON_TYP,C.SCLIENAME,C.SFIRSTNAME,C.SLASTNAME,
      C.SLASTNAME2
      FROM CLIENT C
      LEFT OUTER JOIN (SELECT SCLIENT,NLOCAL,NPROVINCE,NMUNICIPALITY,
                              NCOUNTRY,SKEYADDRESS,SSTREET
                        FROM ADDRESS ADRR
                        WHERE ADRR.NRECOWNER = 2
                          AND ADRR.SRECTYPE = 2
                          AND ADRR.DNULLDATE IS NULL
                          AND TRIM(ADRR.SKEYADDRESS) || TO_CHAR(ADRR.DCOMPDATE, 'YYYYMMDDHH24MMSS') || TO_CHAR(ADRR.DEFFECDATE, 'YYYYMMDDHH24MMSS') =
                              (SELECT \*+INDEX (AT XIF2110ADDRESS)*\
                                MAX(TRIM(AT.SKEYADDRESS) || TO_CHAR(AT.DCOMPDATE, 'YYYYMMDDHH24MMSS') || TO_CHAR(AT.DEFFECDATE, 'YYYYMMDDHH24MMSS'))
                                 FROM ADDRESS AT
                                WHERE AT.SCLIENT = ADRR.SCLIENT
                                  AND AT.NRECOWNER = 2
                                  AND AT.SRECTYPE = 2
                                  AND AT.DNULLDATE IS NULL)

                       ) ADR
         ON ADR.SCLIENT = C.SCLIENT
       LEFT JOIN PROVINCE DPTO
         ON DPTO.NPROVINCE = ADR.NPROVINCE
       LEFT JOIN TAB_LOCAT PROV
         ON PROV.NLOCAL = ADR.NLOCAL
       LEFT JOIN MUNICIPALITY DIST
         ON DIST.NMUNICIPALITY = ADR.NMUNICIPALITY
       LEFT OUTER JOIN (SELECT
                            \*+INDEX (PHR IDX_PHONE_1)*\
                             SPHONE,NKEYPHONES,SKEYADDRESS,
                             DCOMPDATE,DEFFECDATE
                             FROM PHONES PHR
                             WHERE PHR.NRECOWNER = 2
                               AND PHR.DNULLDATE IS NULL
                               AND TRIM(PHR.SKEYADDRESS) || PHR.NKEYPHONES || TO_CHAR(PHR.DCOMPDATE, 'YYYYMMDDHH24MMSS') || TO_CHAR(PHR.DEFFECDATE, 'YYYYMMDDHH24MMSS') =
                                   (SELECT \*+INDEX (PHT IDX_PHONE_1)*\
                                     MAX(TRIM(PHT.SKEYADDRESS) || PHT.NKEYPHONES || TO_CHAR(PHT.DCOMPDATE, 'YYYYMMDDHH24MMSS') || TO_CHAR(PHT.DEFFECDATE, 'YYYYMMDDHH24MMSS'))
                                      FROM PHONES PHT
                                     WHERE SUBSTR(PHT.SKEYADDRESS, 2, 14) = SUBSTR(PHR.SKEYADDRESS, 2, 14)
                                       AND PHT.NRECOWNER = 2
                                       AND PHT.DNULLDATE IS NULL)

                            ) PH
              ON SUBSTR(PH.SKEYADDRESS, 2, 14) = C.SCLIENT
              LEFT OUTER JOIN EQUI_UBIGEO EQUI
              ON TO_NUMBER(EQUI.COD_UBI_DIS) = dist.NMUNICIPALITY
              AND EQUI.COD_CLI = '11111111111111'
              LEFT OUTER JOIN TBL_TRX_NATIONALITY_ISO NI
              ON C.NNATIONALITY = NI.NNATIONALITY
              AND NI.SACTIVE=1
              LEFT JOIN CLIENT_COMPLEMENT CC
              ON C.SCLIENT = CC.SCLIENT
              LEFT OUTER JOIN CLIENT_IDDOC DCLI
              ON DCLI.SCLIENT = C.SCLIENT
      ) clASEG
      ON clASEG.SCLIENT = P.BENEFICIARIO*/
        LEFT JOIN (SELECT C.NPERSON_TYP,
                          C.SCLIENAME,
                          C.SFIRSTNAME,
                          C.SLASTNAME,
                          C.SLASTNAME2,
                          C.NQ_CHILD,
                          ADR.SSTREET,
                          DPTO.SDESCRIPT AS DEPART,
                          PROV.SDESCRIPT AS PROV,
                          DIST.SDESCRIPT AS DISTRITO,
                          PH.SPHONE,
                          C.STAX_CODE,
                          DIST.NMUNICIPALITY,
                          CC.CTA_BCRIA_CONTRATANTE,
                          CC.COD_EMP,
                          CC.CIIU_CONTRATANTE,
                          CC.COD_OCUPACION_ASEGU,
                          C.SCLIENT,
                          EQUI.COD_UBI_CLI,
                          DCLI.NIDDOC_TYPE AS TIP_DOC,
                          DCLI.SIDDOC AS NUM_DOC,
                          C.NNATIONALITY,
                          NI.SCOD_COUNTRY_ISO,
                          CASE
                            WHEN SUBSTR(C.SCLIENT, 1, 2) = '01' THEN
                             CASE
                               WHEN SUBSTR(C.SCLIENT, 4, 2) = '20' THEN
                                '3' --'Persona juridica'
                               ELSE
                                '1' --'persona natural con negocio'
                             END
                            ELSE
                             '1' --'Persona natural'
                          END AS TIPO_PERSONA,
                          CASE
                            WHEN DCLI.NIDDOC_TYPE = 2 THEN
                             '1'
                            WHEN DCLI.NIDDOC_TYPE = 4 THEN
                             '2'
                            WHEN DCLI.NIDDOC_TYPE = 6 THEN
                             '5'
                            WHEN DCLI.NIDDOC_TYPE = 1 THEN
                             ' '
                            WHEN DCLI.NIDDOC_TYPE IN (3, 5, 7, 8, 9, 10, 11, 0, 12, 13) THEN
                             '9'
                            ELSE
                             ' '
                          END AS TIPO_DOCUMENTO,
                          COS.NIDOCUP_SBS AS COD_ESPECIALIDAD_SBS
                     FROM CLIENT C
                     LEFT OUTER JOIN (SELECT SCLIENT,
                                            NLOCAL,
                                            NPROVINCE,
                                            NMUNICIPALITY,
                                            NCOUNTRY,
                                            SKEYADDRESS,
                                            SSTREET
                                       FROM ADDRESS ADRR
                                      WHERE ADRR.NRECOWNER = 2
                                        AND ADRR.SRECTYPE = 2
                                        AND ADRR.DNULLDATE IS NULL
                                        AND TRIM(ADRR.SKEYADDRESS) || TO_CHAR(ADRR.DCOMPDATE, 'YYYYMMDDHH24MMSS') || TO_CHAR(ADRR.DEFFECDATE, 'YYYYMMDDHH24MMSS') =
                                            (SELECT /*+INDEX (AT XIF2110ADDRESS)*/
                                              MAX(TRIM(AT.SKEYADDRESS) || TO_CHAR(AT.DCOMPDATE, 'YYYYMMDDHH24MMSS') || TO_CHAR(AT.DEFFECDATE, 'YYYYMMDDHH24MMSS'))
                                               FROM ADDRESS AT
                                              WHERE AT.SCLIENT = ADRR.SCLIENT
                                                AND AT.NRECOWNER = 2
                                                AND AT.SRECTYPE = 2
                                                AND AT.DNULLDATE IS NULL)

                                     ) ADR
                       ON ADR.SCLIENT = C.SCLIENT
                     LEFT JOIN PROVINCE DPTO
                       ON DPTO.NPROVINCE = ADR.NPROVINCE
                     LEFT JOIN TAB_LOCAT PROV
                       ON PROV.NLOCAL = ADR.NLOCAL
                     LEFT JOIN MUNICIPALITY DIST
                       ON DIST.NMUNICIPALITY = ADR.NMUNICIPALITY
                     LEFT OUTER JOIN (SELECT
                                     /*+INDEX (PHR IDX_PHONE_1)*/
                                      SPHONE,
                                      NKEYPHONES,
                                      SKEYADDRESS,
                                      DCOMPDATE,
                                      DEFFECDATE
                                       FROM PHONES PHR
                                      WHERE PHR.NRECOWNER = 2
                                        AND PHR.DNULLDATE IS NULL
                                        AND TRIM(PHR.SKEYADDRESS) || PHR.NKEYPHONES || TO_CHAR(PHR.DCOMPDATE, 'YYYYMMDDHH24MMSS') || TO_CHAR(PHR.DEFFECDATE, 'YYYYMMDDHH24MMSS') =
                                            (SELECT /*+INDEX (PHT IDX_PHONE_1)*/
                                              MAX(TRIM(PHT.SKEYADDRESS) || PHT.NKEYPHONES || TO_CHAR(PHT.DCOMPDATE, 'YYYYMMDDHH24MMSS') || TO_CHAR(PHT.DEFFECDATE, 'YYYYMMDDHH24MMSS'))
                                               FROM PHONES PHT
                                              WHERE SUBSTR(PHT.SKEYADDRESS, 2, 14) = SUBSTR(PHR.SKEYADDRESS, 2, 14)
                                                AND PHT.NRECOWNER = 2
                                                AND PHT.DNULLDATE IS NULL)

                                     ) PH
                       ON SUBSTR(PH.SKEYADDRESS, 2, 14) = C.SCLIENT
                     LEFT OUTER JOIN EQUI_UBIGEO EQUI
                       ON TO_NUMBER(EQUI.COD_UBI_DIS) = DIST.NMUNICIPALITY
                      AND EQUI.COD_CLI = '11111111111111'
                     LEFT OUTER JOIN TBL_TRX_NATIONALITY_ISO NI
                       ON C.NNATIONALITY = NI.NNATIONALITY
                      AND NI.SACTIVE = 1
                     LEFT JOIN CLIENT_COMPLEMENT CC
                       ON C.SCLIENT = CC.SCLIENT
                     LEFT OUTER JOIN CLIENT_IDDOC DCLI
                       ON DCLI.SCLIENT = C.SCLIENT
                     LEFT OUTER JOIN EQUI_UBIGEO EQUI
                       ON TO_NUMBER(EQUI.COD_UBI_DIS) = DIST.NMUNICIPALITY
                      AND EQUI.COD_CLI = '11111111111111'
                     LEFT OUTER JOIN TBL_TRX_NATIONALITY_ISO NI
                       ON C.NNATIONALITY = NI.NNATIONALITY
                      AND NI.SACTIVE = 1
                     LEFT OUTER JOIN TBL_CONFIG_OCUP_SBS COS
                       ON COS.NIDOCUPACION = C.NSPECIALITY
                      AND COS.SORIGEN_BD = 'TIME'
                      AND COS.SACTIVE = '1'

                   ) CLORDEN
          ON CLORDEN.SCLIENT = P.ORDENANTE;
  END NC_VIDA_MAYORES;

  PROCEDURE NC_VIDA_MAYORES_2(P_OPE    NUMBER,
                              P_TC     NUMBER,
                              P_MONTO  NUMBER,
                              P_FECINI VARCHAR2,
                              P_FECFIN VARCHAR2,
                              C_TABLE  OUT MYCURSOR) IS
  BEGIN
    OPEN C_TABLE FOR
      SELECT

       TRIM(TO_CHAR(ROWNUM, '00000000')) AS FILA, --1-vb.801
       '001 ' AS OFICINA, --2
       TRIM(TO_CHAR(ROWNUM, '00000000')) AS OPERACION, --3-vb.802
       RPAD(TRIM(TRIM(TO_CHAR(K.DBILLDATE, 'YYYYMM')) || TRIM(TO_CHAR(DECODE(TRIM(K.NPOLICY), '', 0, K.NPOLICY), '0000')) || TRIM(TO_CHAR(TRIM(K.NCERTIF), '0000000000'))), 20) AS INTERNO, --4
       K.MODALIDAD AS MODALIDAD, --5-vb.827
       '150141' AS OPE_UBIGEO, --6
       RPAD(DECODE(K.DBILLDATE, '', ' ', TO_CHAR(K.DBILLDATE, 'YYYYMMDD')), 8) AS OPE_FECHA, --7
       RPAD(DECODE(K.DCOMPDATE, '', ' ', TO_CHAR(K.DCOMPDATE, 'HH24MISS')), 6) AS OPE_HORA, --8
       RPAD(' ', 10) AS EJE_RELACION, --vb.835
       RPAD(' ', 1) AS EJE_CONDICION, --vb.836
       RPAD(' ', 1) AS EJE_TIPPER, --vb
       RPAD(' ', 1) AS EJE_TIPDOC, --vb
       RPAD(' ', 12) AS EJE_NUMDOC, --vb
       RPAD(' ', 11) AS EJE_NUMRUC, --vb
       RPAD(' ', 40) AS EJE_APEPAT, --vb
       RPAD(' ', 40) AS EJE_APEMAT, --vb
       RPAD(' ', 40) AS EJE_NOMBRES, --vb
       RPAD(' ', 4) AS EJE_OCUPACION, --vb
       --'' AS EJE_CIIU, --vb.845
       RPAD(' ', 6) AS EJE_PAIS,
       --'' AS EJE_DESCIIU, --vb.846
       RPAD(' ', 104) AS EJE_CARGO, --vb.847
       RPAD(' ', 2) AS EJE_PEP,
       RPAD(' ', 150) AS EJE_DOMICILIO, --vb.848
       RPAD(' ', 2) AS EJE_DEPART, --vb.849
       RPAD(' ', 2) AS EJE_PROV, --vb.850
       RPAD(' ', 2) AS EJE_DIST, --vb.851
       RPAD(' ', 40) AS EJE_TELEFONO, --vb.852

       RPAD('1', 1) AS BEN_RELACION, --45
       RPAD('1', 1) AS BEN_CONDICION, --46
       --RPAD(DECODE(SUBSTR(CLBENE.SCLIENT, 1, 2), '02', '2', '01', '3', ' '), 1) AS BEN_TIP_PER, --47
       --05/10/2020--RPAD(DECODE(CLBENE.NPERSON_TYP,'',' ',CLBENE.NPERSON_TYP),1) AS BEN_TIP_PER,
       RPAD(NVL(CLBENE.TIPO_PERSONA_BEN, ' '), 1) AS BEN_TIP_PER,
       --05/10/2020--RPAD(DECODE(CLBENE.NPERSON_TYP,2,DECODE(CLBENE.TIP_DOC_BEN,'',' ',CLBENE.TIP_DOC_BEN),' '), 1) AS BEN_TIP_DOC,
       RPAD(NVL(CLBENE.TIPO_DOCUMENTO_BEN, ' '), 1) AS BEN_TIP_DOC,
       --RPAD(DECODE(SUBSTR(CLBENE.SCLIENT, 1, 2), '02', '1', '01', '9', ' '), 1) AS BEN_TIP_DOC, --48
       --RPAD(DECODE(CLBENE.NUM_DOC_BEN,'',' ',CLBENE.NUM_DOC_BEN), 12) AS BEN_NUM_DOC,
       RPAD(CASE
              WHEN CLBENE.TIPO_PERSONA_BEN = '1' OR CLBENE.TIPO_PERSONA_BEN = '2' THEN
               CLBENE.NUM_DOC_BEN
              ELSE
               ' '
            END,
            12) AS BEN_NUM_DOC,
       --RPAD(DECODE(SUBSTR(CLBENE.SCLIENT, 1, 2), '02', RIGHT(CLBENE.SCLIENT, 8), '01', ' ', RIGHT(CLBENE.SCLIENT, 12)), 12) AS BEN_NUM_DOC, --49
       --RPAD(DECODE(SUBSTR(CLBENE.SCLIENT, 1, 2), '02', CLBENE.STAX_CODE, '01', RIGHT(CLBENE.SCLIENT, 11), ' '), 11) AS BEN_NUM_RUC, --50
       RPAD(CASE
              WHEN CLBENE.TIPO_PERSONA_BEN = '3' OR CLBENE.TIPO_PERSONA_BEN = '4' THEN
               RIGHT(CLBENE.SCLIENT, 11)
              ELSE
               ' '
            END,
            12) AS BEN_NUM_RUC,
       RPAD(REPLACE(REPLACE(REPLACE(CASE
                                      WHEN CLBENE.TIPO_PERSONA_BEN = '1' OR CLBENE.TIPO_PERSONA_BEN = '2' THEN
                                       NVL(CLBENE.SLASTNAME, ' ')
                                      WHEN CLBENE.TIPO_PERSONA_BEN = '3' OR CLBENE.TIPO_PERSONA_BEN = '4' THEN
                                       NVL(CLBENE.SCLIENAME, ' ')
                                      ELSE
                                       ' '
                                    END,
                                    '?',
                                    '#'),
                            'Ñ',
                            '#'),
                    'ñ',
                    '#'),
            120) AS BEN_APEPAT,
       --RPAD(REPLACE(REPLACE(REPLACE(DECODE(SUBSTR(CLBENE.SCLIENT, 1, 2), '02', CLBENE.SLASTNAME, '01', CLBENE.SCLIENAME, ' '), '?', '#'), 'Ñ', '#'), 'ñ', '#'), 120) AS BEN_APEPAT, --51
       RPAD(REPLACE(REPLACE(REPLACE(DECODE(CLBENE.SLASTNAME2, '', ' ', CLBENE.SLASTNAME2), '?', '#'), 'Ñ', '#'), 'ñ', '#'), 40) AS BEN_APEMAT, --52
       RPAD(REPLACE(REPLACE(REPLACE(DECODE(CLBENE.SFIRSTNAME, '', ' ', CLBENE.SFIRSTNAME), '?', '#'), 'Ñ', '#'), 'ñ', '#'), 40) AS BEN_NOMBRES, --53

       RPAD(NVL(CLBENE.COD_ESPECIALIDAD_SBS_BEN, ' '), 4) AS BEN_OCUPACION, --54
       --RPAD('' AS Ben_CIIU,--55-YA NO SE SOLICITA
       --RPAD('PE', 6) /*tb66.sdescript*/ AS BEN_PAIS, --55
       RPAD(DECODE(CLBENE.SCOD_COUNTRY_ISO, '', ' ', CLBENE.SCOD_COUNTRY_ISO), 6) AS BEN_PAIS,
       --RPAD('' AS Ben_Des_CIIU,--56-YA NO SE SOLICITA
       RPAD(' ', 104) AS BEN_CARGO, --56-NO ES OBLIGATORIO
       RPAD(' ', 2) AS BEN_PEP, --57-NO ES OBLIGATORIO
       --CLBENE.SSTREET AS DIRECCION_B,
       RPAD(REPLACE(REPLACE(REPLACE(DECODE(CLBENE.SSTREET, '', ' ', TRIM(CLBENE.SSTREET)) || ' ' || TRIM(CLBENE.DISTRITO) || ' ' || TRIM(CLBENE.PROV) || ' ' || TRIM(CLBENE.DEPART), '?', '#'),
                            'Ñ',
                            '#'),
                    'ñ',
                    '#'),
            150) AS BEN_DOMICILIO, --58
       --TO_NUMBER(CLBENE.COD_UBI_CLI) AS BEN_UBIGEO,
       RPAD(DECODE(CLBENE.NMUNICIPALITY, '', '15', SUBSTR(TRIM(TO_CHAR((CLBENE.COD_UBI_CLI), '000000')), 1, 2)), 2) AS BEN_DEPART, --59
       RPAD(DECODE(CLBENE.NMUNICIPALITY, '', '01', SUBSTR(TRIM(TO_CHAR((CLBENE.COD_UBI_CLI), '000000')), 3, 2)), 2) AS BEN_PROV, --60
       RPAD(DECODE(CLBENE.NMUNICIPALITY, '', '41', SUBSTR(TRIM(TO_CHAR((CLBENE.COD_UBI_CLI), '000000')), 5, 2)), 2) AS BEN_DIST, --61
       RPAD(DECODE(CLBENE.SPHONE, '', ' ', CLBENE.SPHONE), 40) AS BEN_TELEFONO, --62
       RPAD('2', 1) AS DAT_TIPFON, --63
       RPAD('34', 2) AS DAT_TIPOPE, --64
       RPAD(' ', 40) AS DAT_DESOPE, --65
       RPAD(' ', 80) AS DAT_ORIFON, --66
       RPAD(DECODE(K.NCURRENCY, '1', 'PEN', 'USD'), 3) AS DAT_MONOPE, --67
       RPAD(' ', 3) AS DAT_MONOPE_A, --68
       RPAD(TO_CHAR(CAST((K.NAMOUNT) AS NUMBER(15, 2)), '000000000000000.00'), 29) AS DAT_MTOOPE, --MTO_PENSIONGAR , MTO_PRIUNI 69
       RPAD(' ', 30) AS DAT_MTOOPEA, --MTO_PENSIONGAR , MTO_PRIUNI 70
       RPAD(' ', 5) AS DAT_COD_ENT_INVO, --71--NO OBLIGATORIO
       RPAD('1', 1) AS DAT_COD_TIP_CTAO, --72--NO OBLIGATORIO
       RPAD('002193116232365', 20) AS DAT_COD_CTAO, --73--NO OBLIGATORIO
       RPAD(' ', 150) AS DAT_ENT_FNC_EXTO, --74--NO OBLIGATORIO
       RPAD(' ', 5) AS DAT_COD_ENT_INVB, --75--NO OBLIGATORIO
       RPAD('1', 1) AS DAT_COD_TIP_CTAB, --76--NO OBLIGATORIO
       RPAD(' ', 20) AS DAT_COD_CTAB, --77--NO OBLIGATORIO
       RPAD(' ', 150) AS DAT_ENT_FNC_EXTB, --78--NO OBLIGATORIO
       RPAD('1', 1) AS DAT_ALCANCE, --79
       RPAD(' ', 2) AS DAT_COD_PAISO, --80--ALCANCE 1, ESTE CAMPO VA EN BLANCO
       RPAD(' ', 2) AS DAT_COD_PAISD, --81--ALCANCE 1, ESTE CAMPO VA EN BLANCO
       RPAD('2', 1) AS DAT_INTOPE, --82--NO OBLIGATORIO
       RPAD('2', 1) AS DAT_FORMA, --83
       RPAD(DECODE(K.SDESCRIPT, '', ' ', K.SDESCRIPT), 40) AS DAT_INFORM, --84--NO OBLIGATORIO
       'PRI' AS ORIGEN

        FROM (

               SELECT
               /*+INDEX(P XPKPOLICY) INDEX(CT XPKCERTIFICAT) INDEX(CL XPKCLIENT) INDEX(M XPKPRODMASTER)*/
                A.NPRODUCT,
                 A.NPOLICY,
                 M.SDESCRIPT AS DESCRIPCIONPRODUCTO,
                 C.DBILLDATE,
                 A.NCERTIF,
                 CT.SCLIENT AS BENEFICIARIO,
                 CL.SCLIENAME,
                 C.NAMOUNT,
                 C.NCURRENCY,
                 P_TC AS TIPO_CAMBIO,
                 TO_CHAR(DECODE(C.NCURRENCY, 1, C.NAMOUNT, 0)) AS SOLES,
                 TO_CHAR(DECODE(C.NCURRENCY, 2, A.NPREMIUM, ROUND(A.NPREMIUM / P_TC, 4))) AS DOLARES,
                 B.DCOMPDATE,
                 A.SCLIENT AS ASEGURADO,
                 T.SDESCRIPT,
                 'U' AS MODALIDAD
                 FROM LIFE       A,
                       CERTIFICAT CT,
                       PREMIUM    B,
                       BILLS      C,
                       POLICY     P,
                       TABLE36    T,
                       PRODMASTER M,
                       TABLE5564  E,
                       CLIENT     CL

                WHERE

                CT.SCERTYPE = A.SCERTYPE
             AND CT.NBRANCH = A.NBRANCH
             AND CT.NPRODUCT = A.NPRODUCT
             AND CT.NPOLICY = A.NPOLICY
             AND CT.NCERTIF = A.NCERTIF
             AND B.NRECEIPT = A.NRECEIPT
             AND C.NINSUR_AREA = B.NINSUR_AREA
             AND C.SBILLING = '1'
             AND C.SBILLTYPE = B.SBILLTYPE
             AND C.NBILLNUM = B.NBILLNUM
             AND P.SCERTYPE = A.SCERTYPE
             AND P.NBRANCH = A.NBRANCH
             AND P.NPRODUCT = A.NPRODUCT
             AND P.NPOLICY = A.NPOLICY
             AND P.NPAYFREQ = T.NPAYFREQ
             AND A.NPRODUCT = M.NPRODUCT
             AND C.NBILLSTAT = E.NBILLSTAT
             AND A.SCLIENT = CL.SCLIENT
             AND A.SCERTYPE = '2'
             AND A.NBRANCH = 1
             AND A.NCERTIF > 0
             AND P.NPAYFREQ = 6
             AND C.SBILLTYPE IN (7, 8)
             AND TRUNC(C.DBILLDATE) BETWEEN P_FECINI AND P_FECFIN
             AND C.NBILLSTAT <> 2
             AND DECODE(C.NCURRENCY, 2, A.NPREMIUM, ROUND(A.NPREMIUM / P_TC, 4)) >= P_MONTO
             AND NOT B.NPRODUCT IN (117, 120, 1000)

               UNION ALL

               SELECT
               /*+INDEX(P XPKPOLICY) INDEX(CT XPKCERTIFICAT) INDEX(CL XPKCLIENT) INDEX(M XPKPRODMASTER)*/
                A.NPRODUCT,
                 A.NPOLICY,
                 M.SDESCRIPT AS DESCRIPCIONPRODUCTO,
                 C.DBILLDATE,
                 A.NCERTIF,
                 CT.SCLIENT AS BENEFICIARIO,
                 CL.SCLIENAME,
                 C.NAMOUNT,
                 C.NCURRENCY,
                 P_TC AS TIPO_CAMBIO,
                 TO_CHAR(DECODE(C.NCURRENCY, 1, C.NAMOUNT, 0)) AS SOLES,
                 TO_CHAR(DECODE(C.NCURRENCY, 2, A.NPREMIUM, ROUND(A.NPREMIUM / P_TC, 4))) AS DOLARES,
                 B.DCOMPDATE,
                 A.SCLIENT AS ASEGURADO,
                 T.SDESCRIPT,
                 'U' AS MODALIDAD
                 FROM LIFE A,
                       CERTIFICAT CT,
                       PREMIUM B,
                       BILLS C,
                       POLICY P,
                       TABLE36 T,
                       TABLE5564 E,
                       PRODMASTER M,
                       CLIENT CL,
                       (SELECT B.SCLIENT,
                               B.NRECEIPT,
                               SUM(DECODE(A.NCURRENCY, 2, A.NAMOUNT, ROUND(A.NAMOUNT / P_TC, 4))) AS MONTO
                          FROM BILLS A
                         INNER JOIN PREMIUM B
                            ON B.NINSUR_AREA = A.NINSUR_AREA
                           AND B.SBILLTYPE = A.SBILLTYPE
                           AND B.NBILLNUM = A.NBILLNUM
                         WHERE B.NPRODUCT IN (117, 120)
                           AND A.NBILLSTAT <> 2
                           AND A.SBILLTYPE IN (7, 8)
                           AND TRUNC(A.DBILLDATE) BETWEEN P_FECINI AND P_FECFIN
                         GROUP BY B.SCLIENT,
                                  B.NRECEIPT) X

                WHERE CT.SCERTYPE = A.SCERTYPE
                  AND CT.NBRANCH = A.NBRANCH
                  AND CT.NPRODUCT = A.NPRODUCT
                  AND CT.NPOLICY = A.NPOLICY
                  AND CT.NCERTIF = A.NCERTIF
                  AND A.NRECEIPT = B.NRECEIPT
                  AND C.NINSUR_AREA = B.NINSUR_AREA
                  AND C.SBILLING = '1'
                  AND C.SBILLTYPE = B.SBILLTYPE
                  AND C.NBILLNUM = B.NBILLNUM
                  AND P.SCERTYPE = A.SCERTYPE
                  AND P.NBRANCH = A.NBRANCH
                  AND P.NPRODUCT = A.NPRODUCT
                  AND P.NPOLICY = A.NPOLICY
                  AND P.NPAYFREQ = T.NPAYFREQ
                  AND A.NPRODUCT = M.NPRODUCT
                  AND C.NBILLSTAT = E.NBILLSTAT
                  AND A.SCLIENT = CL.SCLIENT
                  AND X.SCLIENT = CT.SCLIENT
                  AND X.NRECEIPT = B.NRECEIPT
                  AND X.MONTO > P_MONTO

                  AND A.SCERTYPE = '2'
                  AND A.NBRANCH = 1
                  AND A.NCERTIF > 0
                  AND P.NPAYFREQ = 6
                  AND C.SBILLTYPE IN (7, 8)
                  AND TRUNC(C.DBILLDATE) BETWEEN P_FECINI AND P_FECFIN
                  AND C.NBILLSTAT <> 2

               UNION ALL
               SELECT
               /*+INDEX(P XPKPOLICY) INDEX(CT XPKCERTIFICAT) INDEX(CL XPKCLIENT) INDEX(M XPKPRODMASTER)*/
                CT.NPRODUCT,
                 CT.NPOLICY,
                 M.SDESCRIPT AS DESCRIPCIONPRODUCTO,
                 C.DBILLDATE,
                 CT.NCERTIF,
                 CT.SCLIENT AS BENEFICIARIO,
                 CL.SCLIENAME,
                 C.NAMOUNT,
                 C.NCURRENCY,
                 P_TC AS TIPO_CAMBIO,
                 TO_CHAR(DECODE(C.NCURRENCY, 1, C.NAMOUNT, 0)) AS SOLES,
                 TO_CHAR(DECODE(C.NCURRENCY, 2, B.NPREMIUM, ROUND(B.NPREMIUM / P_TC, 4))) AS DOLARES,
                 B.DCOMPDATE,
                 B.SCLIENT AS ASEGURADO,
                 T.SDESCRIPT,
                 'U' AS MODALIDAD
                 FROM CERTIFICAT CT
                INNER JOIN PREMIUM B
                   ON B.SCERTYPE = CT.SCERTYPE
                  AND B.NBRANCH = CT.NBRANCH
                  AND B.NPRODUCT = CT.NPRODUCT
                  AND B.NPOLICY = CT.NPOLICY --and ct.nreceipt=b.nreceipt
                  AND B.SBILLTYPE IN (7, 8)
                INNER JOIN BILLS C
                   ON C.NINSUR_AREA = B.NINSUR_AREA
                  AND C.SBILLING = '1'
                  AND C.SBILLTYPE = B.SBILLTYPE
                  AND C.NBILLNUM = B.NBILLNUM
                INNER JOIN POLICY P
                   ON P.SCERTYPE = B.SCERTYPE
                  AND P.NBRANCH = B.NBRANCH
                  AND P.NPRODUCT = B.NPRODUCT
                  AND P.NPOLICY = B.NPOLICY
                INNER JOIN TABLE36 T
                   ON P.NPAYFREQ = T.NPAYFREQ
                INNER JOIN PRODMASTER M
                   ON B.NPRODUCT = M.NPRODUCT
                INNER JOIN TABLE5564 E
                   ON C.NBILLSTAT = E.NBILLSTAT
                INNER JOIN CLIENT CL
                   ON B.SCLIENT = CL.SCLIENT
                WHERE CT.SCERTYPE = '2'
                  AND CT.NBRANCH = 1
                  AND CT.NCERTIF > 0
                  AND P.NPAYFREQ = 6
                  AND TRUNC(C.DBILLDATE) BETWEEN P_FECINI AND P_FECFIN
                  AND C.NBILLSTAT <> 2
                  AND DECODE(C.NCURRENCY, 2, B.NPREMIUM, ROUND(B.NPREMIUM / P_TC, 4)) >= P_MONTO
                  AND B.NPRODUCT IN (1000)

               -------
               -------
               UNION ALL
               ---
               SELECT /*+INDEX(M IDX$$_498B10001) INDEX(T XPKTABLE36)*/

                L.NPRODUCT,
                 L.NPOLICY,
                 M.SDESCRIPT   AS DESCRIPCIONPRODUCTO,
                 B.DBILLDATE,
                 L.NCERTIF,
                 C.SCLIENT     AS BENEFICIARIO,
                 CLI.SCLIENAME,
                 B.NAMOUNT,
                 B.NCURRENCY,
                 P_TC          AS TIPO_CAMBIO,
                 --TO_CHAR(SUM(DECODE(B.NCURRENCY, 1, B.NAMOUNT, 0)))
                 '' AS SOLES,
                 --TO_CHAR(SUM(DECODE(B.NCURRENCY, 2, B.NAMOUNT, ROUND(B.NAMOUNT / P_TC, 4))))
                 '' AS DOLARES,
                 P.DCOMPDATE,
                 L.SCLIENT AS ASEGURADO,
                 T.SDESCRIPT,
                 'M' AS MODALIDAD
                 FROM PREMIUM    P, --B
                       BILLS      B, --C
                       POLICY     PO, --P
                       LIFE       L, --A
                       CERTIFICAT C, --CT
                       TABLE36    T, --T
                       PRODMASTER M,
                       TABLE5564  E,
                       CLIENT     CLI --CL
                WHERE C.SCERTYPE = L.SCERTYPE
                  AND C.NBRANCH = L.NBRANCH
                  AND C.NPRODUCT = L.NPRODUCT
                  AND C.NPOLICY = L.NPOLICY
                  AND C.NCERTIF = L.NCERTIF
                  AND P.NRECEIPT = L.NRECEIPT
                  AND B.NINSUR_AREA = P.NINSUR_AREA
                  AND B.SBILLING = '1'
                  AND B.SBILLTYPE = P.SBILLTYPE
                  AND B.NBILLNUM = P.NBILLNUM
                  AND B.SBILLTYPE IN (7, 8)
                  AND PO.SCERTYPE = L.SCERTYPE
                  AND PO.NBRANCH = L.NBRANCH
                  AND PO.NPRODUCT = L.NPRODUCT
                  AND PO.NPAYFREQ = T.NPAYFREQ
                  AND PO.NPOLICY = L.NPOLICY
                  AND L.NPRODUCT = M.NPRODUCT
                  AND B.NBILLSTAT = E.NBILLSTAT
                  AND L.SCLIENT = CLI.SCLIENT
                  AND L.SCERTYPE = '2'
                  AND L.NBRANCH = 1
                  AND L.NCERTIF > 0
                  AND PO.NPAYFREQ = 6
                  AND TRUNC(B.DBILLDATE) BETWEEN P_FECINI AND P_FECFIN
                  AND B.NBILLSTAT <> 2
                  AND DECODE(B.NCURRENCY, 2, L.NPREMIUM, ROUND(L.NPREMIUM / P_TC, 4)) >= P_MONTO
                  AND L.NPRODUCT != 1000

                  AND L.SCLIENT IN (SELECT X.SCLIENT
                                      FROM (SELECT /*+INDEX(AA XPKLIFE,XIDXNRECEIPT) INDEX(BB XIE6PREMIUM,IDX1PREMIUM)*/
                                             AA.NPOLICY,
                                             AA.SCLIENT
                                              FROM LIFE    AA,
                                                   PREMIUM BB,
                                                   BILLS   CC,
                                                   POLICY  PP,
                                                   TABLE36 TT

                                             WHERE BB.NRECEIPT = AA.NRECEIPT
                                               AND CC.NINSUR_AREA = BB.NINSUR_AREA
                                               AND CC.SBILLING = '1'
                                               AND CC.SBILLTYPE = BB.SBILLTYPE
                                               AND CC.NBILLNUM = BB.NBILLNUM
                                               AND CC.SBILLTYPE IN (7, 8)
                                               AND PP.SCERTYPE = AA.SCERTYPE
                                               AND PP.NBRANCH = AA.NBRANCH
                                               AND PP.NPRODUCT = AA.NPRODUCT
                                               AND PP.NPOLICY = AA.NPOLICY
                                               AND PP.NPAYFREQ = TT.NPAYFREQ
                                               AND AA.SCERTYPE = '2'
                                               AND AA.NBRANCH = 1
                                               AND AA.NCERTIF > 0
                                               AND PP.NPAYFREQ = 6
                                               AND TRUNC(CC.DBILLDATE) BETWEEN P_FECINI AND P_FECFIN
                                               AND CC.NBILLSTAT <> 2
                                               AND DECODE(CC.NCURRENCY, 2, AA.NPREMIUM, ROUND(AA.NPREMIUM / P_TC, 4)) >= P_MONTO
                                               AND AA.NPRODUCT != 1000
                                             GROUP BY AA.NPOLICY,
                                                      AA.SCLIENT
                                             ORDER BY 1,
                                                      2) X
                                     GROUP BY X.SCLIENT
                                    HAVING COUNT(*) > 1)

                GROUP BY L.NPRODUCT,
                          L.NPOLICY,
                          M.SDESCRIPT,
                          B.DBILLDATE,
                          L.NCERTIF,
                          C.SCLIENT,
                          CLI.SCLIENAME,
                          B.NAMOUNT,
                          B.NCURRENCY,
                          P_TC,
                          --SOLES,
                          --DOLARES,
                          P.DCOMPDATE,
                          L.SCLIENT,
                          T.SDESCRIPT
               --****** Frecuente
               UNION ALL
               SELECT /*+INDEX(M IDX$$_498B10001) INDEX(T XPKTABLE36)*/
                L.NPRODUCT,
                 L.NPOLICY,
                 M.SDESCRIPT AS DESCRIPCIONPRODUCTO,
                 B.DBILLDATE,
                 L.NCERTIF,
                 C.SCLIENT AS BENEFICIARIO,
                 CLI.SCLIENAME,
                 B.NAMOUNT,
                 B.NCURRENCY,
                 P_TC AS TIPO_CAMBIO,
                 '' AS SOLES,
                 '' AS DOLARES,
                 B.DCOMPDATE,
                 L.SCLIENT AS ASEGURADO,
                 T.SDESCRIPT,
                 'M' AS MODALIDAD
                 FROM PREMIUM    P, --B
                       BILLS      B, --C
                       POLICY     PO, --P
                       LIFE       L, --A
                       CERTIFICAT C, --CT
                       TABLE36    T, --T
                       PRODMASTER M,
                       TABLE5564  E,
                       CLIENT     CLI --CL

                WHERE C.SCERTYPE = L.SCERTYPE
                  AND C.NBRANCH = L.NBRANCH
                  AND C.NPRODUCT = L.NPRODUCT
                  AND C.NPOLICY = L.NPOLICY
                  AND C.NCERTIF = L.NCERTIF
                  AND P.NRECEIPT = L.NRECEIPT
                  AND PO.NBRANCH = P.NBRANCH
                  AND PO.NPRODUCT = P.NPRODUCT
                  AND PO.NPOLICY = P.NPOLICY
                  AND B.NINSUR_AREA = P.NINSUR_AREA
                  AND B.SBILLING = '1'
                  AND B.SBILLTYPE = P.SBILLTYPE
                  AND B.NBILLNUM = P.NBILLNUM
                  AND PO.SCERTYPE = L.SCERTYPE
                  AND PO.NBRANCH = L.NBRANCH
                  AND PO.NPRODUCT = L.NPRODUCT
                  AND PO.NPOLICY = L.NPOLICY
                  AND PO.NPAYFREQ = T.NPAYFREQ
                  AND L.NPRODUCT = M.NPRODUCT
                  AND B.NBILLSTAT = E.NBILLSTAT
                  AND CLI.SCLIENT = L.SCLIENT
                  AND P.SCLIENT = L.SCLIENT
                  AND PO.SCLIENT = C.SCLIENT
                  AND P.SCLIENT = CLI.SCLIENT
                  AND L.SCLIENT = CLI.SCLIENT
                  AND L.SCERTYPE = '2'
                  AND L.NBRANCH = 1
                  AND L.NCERTIF > 0
                  AND PO.NPAYFREQ <> 6
                  AND B.SBILLTYPE IN (7, 8)
                  AND TRUNC(B.DBILLDATE) BETWEEN P_FECINI AND P_FECFIN
                  AND B.NBILLSTAT <> 2
                  AND DECODE(B.NCURRENCY, 2, L.NPREMIUM, ROUND(L.NPREMIUM / P_TC, 4)) >= P_MONTO
                  AND L.NPRODUCT != 1000
                  AND L.SCLIENT IN (SELECT X.SCLIENT
                                      FROM (SELECT AA.NPOLICY,
                                                    AA.SCLIENT
                                               FROM POLICY  PP,
                                                    PREMIUM BB,
                                                    BILLS   CC,
                                                    LIFE    AA,
                                                    TABLE36 TT

                                              WHERE

                                              CC.NINSUR_AREA = BB.NINSUR_AREA
                                           AND CC.SBILLING = '1'
                                           AND CC.SBILLTYPE = BB.SBILLTYPE
                                           AND CC.NBILLNUM = BB.NBILLNUM
                                           AND CC.SBILLTYPE IN (7, 8)
                                           AND BB.NRECEIPT = AA.NRECEIPT
                                           AND BB.NBRANCH = PP.NBRANCH
                                           AND BB.NPRODUCT = PP.NPRODUCT
                                           AND PP.SCERTYPE = BB.SCERTYPE
                                           AND PP.NBRANCH = BB.NBRANCH
                                           AND PP.NPRODUCT = BB.NPRODUCT
                                           AND PP.NPOLICY = AA.NPOLICY
                                           AND TT.NPAYFREQ = PP.NPAYFREQ
                                           AND AA.SCERTYPE = '2'
                                           AND AA.NBRANCH = 1
                                           AND AA.NCERTIF > 0
                                           AND PP.NPAYFREQ <> 6
                                           AND TRUNC(CC.DBILLDATE) BETWEEN P_FECINI AND P_FECFIN
                                           AND CC.NBILLSTAT <> 2
                                           AND DECODE(CC.NCURRENCY, 2, AA.NPREMIUM, ROUND(AA.NPREMIUM / P_TC, 4)) >= P_MONTO
                                           AND AA.NPRODUCT != 1000
                                              GROUP BY AA.NPOLICY,
                                                       AA.SCLIENT
                                              ORDER BY 1,
                                                       2) X
                                     GROUP BY X.SCLIENT
                                    HAVING COUNT(*) > 1)

                GROUP BY L.NPRODUCT,
                          L.NPOLICY,
                          M.SDESCRIPT,
                          B.DBILLDATE,
                          L.NCERTIF,
                          C.SCLIENT,
                          CLI.SCLIENAME,
                          B.NAMOUNT,
                          B.NCURRENCY,
                          P_TC,
                          --SOLES,
                          --DOLARES,
                          B.DCOMPDATE,
                          L.SCLIENT,
                          T.SDESCRIPT

               UNION ALL
               SELECT CT.NPRODUCT,
                       CT.NPOLICY,
                       M.SDESCRIPT AS DESCRIPCIONPRODUCTO,
                       C.DBILLDATE,
                       CT.NCERTIF,
                       CT.SCLIENT AS BENEFICIARIO,
                       CL.SCLIENAME,
                       C.NAMOUNT,
                       C.NCURRENCY,
                       P_TC AS TIPO_CAMBIO,
                       '' AS SOLES,
                       '' AS DOLARES,
                       B.DCOMPDATE,
                       CT.SCLIENT AS ASEGURADO,
                       T.SDESCRIPT,
                       'M' AS MODALIDAD
                 FROM CERTIFICAT CT
                INNER JOIN PREMIUM B
                   ON B.SCERTYPE = CT.SCERTYPE
                  AND B.NBRANCH = CT.NBRANCH
                  AND B.NPRODUCT = CT.NPRODUCT
                  AND B.NPOLICY = CT.NPOLICY
                  AND B.SBILLTYPE IN (7, 8)
                INNER JOIN BILLS C
                   ON C.NINSUR_AREA = B.NINSUR_AREA
                  AND C.SBILLING = '1'
                  AND C.SBILLTYPE = B.SBILLTYPE
                  AND C.NBILLNUM = B.NBILLNUM
                INNER JOIN POLICY P
                   ON P.SCERTYPE = CT.SCERTYPE
                  AND P.NBRANCH = CT.NBRANCH
                  AND P.NPRODUCT = CT.NPRODUCT
                  AND P.NPOLICY = CT.NPOLICY
                INNER JOIN TABLE36 T
                   ON P.NPAYFREQ = T.NPAYFREQ
                INNER JOIN PRODMASTER M
                   ON CT.NPRODUCT = M.NPRODUCT
                INNER JOIN TABLE5564 E
                   ON C.NBILLSTAT = E.NBILLSTAT
                INNER JOIN CLIENT CL
                   ON CT.SCLIENT = CL.SCLIENT
                WHERE CT.SCERTYPE = '2'
                  AND CT.NBRANCH = 1
                  AND CT.NCERTIF > 0
                  AND P.NPAYFREQ = 6
                  AND TRUNC(C.DBILLDATE) BETWEEN P_FECINI AND P_FECFIN
                  AND C.NBILLSTAT <> 2
                  AND DECODE(C.NCURRENCY, 2, B.NPREMIUM, ROUND(B.NPREMIUM / P_TC, 4)) >= P_MONTO
                  AND B.NPRODUCT = 1000
                  AND B.SCLIENT IN (SELECT X.SCLIENT
                                      FROM (SELECT B.NPOLICY,
                                                   B.SCLIENT
                                              FROM PREMIUM B
                                             INNER JOIN BILLS C
                                                ON C.NINSUR_AREA = B.NINSUR_AREA
                                               AND C.SBILLING = '1'
                                               AND C.SBILLTYPE = B.SBILLTYPE
                                               AND C.NBILLNUM = B.NBILLNUM
                                             INNER JOIN POLICY P
                                                ON P.SCERTYPE = B.SCERTYPE
                                               AND P.NBRANCH = B.NBRANCH
                                               AND P.NPRODUCT = B.NPRODUCT
                                               AND P.NPOLICY = B.NPOLICY
                                             INNER JOIN TABLE36 T
                                                ON P.NPAYFREQ = T.NPAYFREQ
                                             WHERE B.SCERTYPE = '2'
                                               AND B.NBRANCH = 1
                                               AND B.NCERTIF > 0
                                               AND P.NPAYFREQ = 6
                                               AND C.SBILLTYPE IN (7, 8)
                                               AND TRUNC(C.DBILLDATE) BETWEEN P_FECINI AND P_FECFIN
                                               AND C.NBILLSTAT <> 2
                                               AND DECODE(C.NCURRENCY, 2, B.NPREMIUM, ROUND(B.NPREMIUM / P_TC, 4)) >= P_MONTO
                                               AND B.NPRODUCT = 1000
                                             GROUP BY B.NPOLICY,
                                                      B.SCLIENT
                                             ORDER BY 1,
                                                      2) X
                                     GROUP BY X.SCLIENT
                                    HAVING COUNT(*) > 1)
                GROUP BY CT.NPRODUCT,
                          CT.NPOLICY,
                          M.SDESCRIPT,
                          C.DBILLDATE,
                          CT.NCERTIF,
                          B.SCLIENT,
                          CT.SCLIENT,
                          CL.SCLIENAME,
                          C.NAMOUNT,
                          C.NCURRENCY,
                          --SOLES,
                          --DOLARES,
                          B.DCOMPDATE,
                          T.SDESCRIPT) K

        LEFT OUTER JOIN (SELECT
                         /*+INDEX(EQUI SYS_C00167813) INDEX(c XPKPROVINCE) INDEX(d XPKTAB_LOCAT) INDEX(e XPKMUNICIPALITY) INDEX(cc CLIENT_COMPLEMENT_PK)*/
                          A.NPERSON_TYP,
                          A.SCLIENAME,
                          A.SFIRSTNAME,
                          A.SLASTNAME,
                          A.SLASTNAME2,
                          A.NQ_CHILD,
                          ADR.SSTREET,
                          C.SDESCRIPT AS DEPART,
                          D.SDESCRIPT AS PROV,
                          E.SDESCRIPT AS DISTRITO,
                          PH.SPHONE,
                          A.STAX_CODE,
                          E.NMUNICIPALITY,
                          CC.CTA_BCRIA_CONTRATANTE,
                          CC.COD_EMP,
                          CC.CIIU_CONTRATANTE,
                          CC.COD_OCUPACION_ASEGU,
                          A.SCLIENT,
                          EQUI.COD_UBI_CLI,
                          DCLI.NIDDOC_TYPE AS TIP_DOC_BEN,
                          DCLI.SIDDOC AS NUM_DOC_BEN,
                          A.NNATIONALITY,
                          NI.SCOD_COUNTRY_ISO,
                          CASE
                            WHEN SUBSTR(A.SCLIENT, 1, 2) = '01' THEN
                             CASE
                               WHEN SUBSTR(A.SCLIENT, 4, 2) = '20' THEN
                                '3' --'Persona juridica'
                               ELSE
                                '1' --'persona natural con negocio'
                             END
                            ELSE
                             '1' --'Persona natural'
                          END AS TIPO_PERSONA_BEN,
                          CASE
                            WHEN DCLI.NIDDOC_TYPE = 2 THEN
                             '1'
                            WHEN DCLI.NIDDOC_TYPE = 4 THEN
                             '2'
                            WHEN DCLI.NIDDOC_TYPE = 6 THEN
                             '5'
                            WHEN DCLI.NIDDOC_TYPE = 1 THEN
                             ' '
                            WHEN DCLI.NIDDOC_TYPE = 3 OR DCLI.NIDDOC_TYPE = 5 OR DCLI.NIDDOC_TYPE = 7 OR DCLI.NIDDOC_TYPE = 8 OR DCLI.NIDDOC_TYPE = 9 OR DCLI.NIDDOC_TYPE = 10 OR DCLI.NIDDOC_TYPE = 11 OR
                                 DCLI.NIDDOC_TYPE = 0 OR DCLI.NIDDOC_TYPE = 12 OR DCLI.NIDDOC_TYPE = 13 THEN
                             '9'
                            ELSE
                             ' '
                          END AS TIPO_DOCUMENTO_BEN,
                          COS.NIDOCUP_SBS AS COD_ESPECIALIDAD_SBS_BEN
                           FROM CLIENT A

                           LEFT OUTER JOIN (SELECT SCLIENT,
                                                  NLOCAL,
                                                  NPROVINCE,
                                                  NMUNICIPALITY,
                                                  NCOUNTRY,
                                                  SKEYADDRESS,
                                                  SSTREET
                                             FROM ADDRESS ADRR
                                            WHERE ADRR.NRECOWNER = 2
                                              AND ADRR.SRECTYPE = 2
                                              AND ADRR.DNULLDATE IS NULL
                                              AND TRIM(ADRR.SKEYADDRESS) || TO_CHAR(ADRR.DCOMPDATE, 'YYYYMMDDHH24MMSS') || TO_CHAR(ADRR.DEFFECDATE, 'YYYYMMDDHH24MMSS') =
                                                  (SELECT /*+INDEX (AT XIF2110ADDRESS)*/
                                                    MAX(TRIM(AT.SKEYADDRESS) || TO_CHAR(AT.DCOMPDATE, 'YYYYMMDDHH24MMSS') || TO_CHAR(AT.DEFFECDATE, 'YYYYMMDDHH24MMSS'))
                                                     FROM ADDRESS AT
                                                    WHERE AT.SCLIENT = ADRR.SCLIENT
                                                      AND AT.NRECOWNER = 2
                                                      AND AT.SRECTYPE = 2
                                                      AND AT.DNULLDATE IS NULL)

                                           ) ADR
                             ON ADR.SCLIENT = A.SCLIENT

                           LEFT OUTER JOIN (SELECT
                                           /*+INDEX (PHR IDX_PHONE_1)*/
                                            SPHONE,
                                            NKEYPHONES,
                                            SKEYADDRESS,
                                            DCOMPDATE,
                                            DEFFECDATE
                                             FROM PHONES PHR
                                            WHERE PHR.NRECOWNER = 2
                                              AND PHR.DNULLDATE IS NULL
                                              AND TRIM(PHR.SKEYADDRESS) || PHR.NKEYPHONES || TO_CHAR(PHR.DCOMPDATE, 'YYYYMMDDHH24MMSS') || TO_CHAR(PHR.DEFFECDATE, 'YYYYMMDDHH24MMSS') =
                                                  (SELECT /*+INDEX (PHT IDX_PHONE_1)*/
                                                    MAX(TRIM(PHT.SKEYADDRESS) || PHT.NKEYPHONES || TO_CHAR(PHT.DCOMPDATE, 'YYYYMMDDHH24MMSS') || TO_CHAR(PHT.DEFFECDATE, 'YYYYMMDDHH24MMSS'))
                                                     FROM PHONES PHT
                                                    WHERE SUBSTR(PHT.SKEYADDRESS, 2, 14) = SUBSTR(PHR.SKEYADDRESS, 2, 14)
                                                      AND PHT.NRECOWNER = 2
                                                      AND PHT.DNULLDATE IS NULL)

                                           ) PH
                             ON SUBSTR(PH.SKEYADDRESS, 2, 14) = A.SCLIENT

                           LEFT OUTER JOIN PROVINCE C
                             ON C.NPROVINCE = ADR.NPROVINCE
                           LEFT OUTER JOIN TAB_LOCAT D
                             ON D.NLOCAL = ADR.NLOCAL
                           LEFT OUTER JOIN MUNICIPALITY E
                             ON E.NMUNICIPALITY = ADR.NMUNICIPALITY
                           LEFT OUTER JOIN CLIENT_COMPLEMENT CC
                             ON A.SCLIENT = CC.SCLIENT
                           LEFT OUTER JOIN CLIENT_IDDOC DCLI
                             ON DCLI.SCLIENT = A.SCLIENT
                           LEFT OUTER JOIN EQUI_UBIGEO EQUI
                             ON TO_NUMBER(EQUI.COD_UBI_DIS) = E.NMUNICIPALITY
                            AND EQUI.COD_CLI = '11111111111111'
                           LEFT OUTER JOIN TBL_TRX_NATIONALITY_ISO NI
                             ON A.NNATIONALITY = NI.NNATIONALITY
                            AND NI.SACTIVE = 1
                           LEFT OUTER JOIN TBL_CONFIG_OCUP_SBS COS
                             ON COS.NIDOCUPACION = A.NSPECIALITY
                            AND COS.SORIGEN_BD = 'TIME'
                            AND COS.SACTIVE = '1') CLBENE
          ON CLBENE.SCLIENT = K.BENEFICIARIO;
  END NC_VIDA_MAYORES_2;

  PROCEDURE COMISIONES(P_OPE    NUMBER,
                       P_TC     NUMBER,
                       P_MONTO  NUMBER,
                       P_FECINI VARCHAR2,
                       P_FECFIN VARCHAR2,
                       C_TABLE  OUT MYCURSOR) IS

    V_SCLIENT_ORD CLIENT.SCLIENT%TYPE;
    V_SPHONE_ORD  PHONES.SPHONE%TYPE;

    V_SCLIENT_AD        CLIENT.SCLIENT%TYPE;
    V_SDIRECCION_ORD    ADDRESS_CLIENT.SDESDIREBUSQ%TYPE;
    V_STI_DIRE          ADDRESS_CLIENT.STI_DIRE%TYPE;
    V_SNOM_DIRECCION    ADDRESS_CLIENT.SNOM_DIRECCION%TYPE;
    V_SNUM_DIRECCION    ADDRESS_CLIENT.SNUM_DIRECCION%TYPE;
    V_STI_BLOCKCHALET   ADDRESS_CLIENT.STI_BLOCKCHALET%TYPE;
    V_SBLOCKCHALET      ADDRESS_CLIENT.SBLOCKCHALET%TYPE;
    V_STI_INTERIOR      ADDRESS_CLIENT.STI_INTERIOR%TYPE;
    V_SNUM_INTERIOR     ADDRESS_CLIENT.SNUM_INTERIOR%TYPE;
    V_STI_CJHT          ADDRESS_CLIENT.STI_CJHT%TYPE;
    V_SNOM_CJHT         ADDRESS_CLIENT.SNOM_CJHT%TYPE;
    V_SETAPA            ADDRESS_CLIENT.SETAPA%TYPE;
    V_SMANZANA          ADDRESS_CLIENT.SMANZANA%TYPE;
    V_SLOTE             ADDRESS_CLIENT.SLOTE%TYPE;
    V_SREFERENCIA       ADDRESS_CLIENT.SREFERENCIA%TYPE;
    V_NMUNICIPALITY_ORD ADDRESS.NMUNICIPALITY%TYPE;
    V_NPROVINCE_ORD     PROVINCE.NPROVINCE%TYPE;
    V_NLOCAL_ORD        TAB_LOCAT.NLOCAL%TYPE;
    V_SCLIENAME_ORD     CLIENT.SCLIENAME%TYPE;
    V_SIDDOC_ORD        CLIENT_IDDOC.SIDDOC%TYPE;
    V_OFICINA           VARCHAR2(4);
    V_OPE_UBIGEO        VARCHAR2(6);
    V_ORD_RELACION      VARCHAR2(1);
    V_ORD_CONDICION     VARCHAR2(1);
    V_ORD_TIPPER        VARCHAR2(1);
    V_ORD_PAIS          VARCHAR2(2);
    V_BEN_RELACION      VARCHAR2(1);
    V_BEN_CONDICION     VARCHAR2(1);
    V_BEN_PAIS          VARCHAR2(2);
    V_DAT_TIPFON        VARCHAR2(1);
    V_DAT_TIPOPE        VARCHAR2(2);
    V_DAT_ALCANCE       VARCHAR2(1);
    V_DAT_FORMA         VARCHAR2(1);
    V_DISTRITO_ORD      MUNICIPALITY.SDESCRIPT%TYPE;
    V_PROVINCIA_ORD     TAB_LOCAT.SDESCRIPT%TYPE;
    V_DEPARTAMENTO_ORD  PROVINCE.SDESCRIPT%TYPE;
    V_ORD_TIPDOC        VARCHAR2(2);

  BEGIN

    BEGIN
      SELECT SVALOR
        INTO V_OFICINA
        FROM LAFT.TBL_CONFIG_REPORTES
       WHERE SORIGEN = 'NC'
         AND SDESCAMPO = 'OFICINA';

      SELECT SVALOR
        INTO V_OPE_UBIGEO
        FROM LAFT.TBL_CONFIG_REPORTES
       WHERE SORIGEN = 'NC'
         AND SDESCAMPO = 'OPE_UBIGEO';

      SELECT SVALOR
        INTO V_ORD_RELACION
        FROM LAFT.TBL_CONFIG_REPORTES
       WHERE SORIGEN = 'NC'
         AND SDESCAMPO = 'ORD_RELACION';

      SELECT SVALOR
        INTO V_ORD_CONDICION
        FROM LAFT.TBL_CONFIG_REPORTES
       WHERE SORIGEN = 'NC'
         AND SDESCAMPO = 'ORD_CONDICION';

      SELECT SVALOR
        INTO V_ORD_TIPPER
        FROM LAFT.TBL_CONFIG_REPORTES
       WHERE SORIGEN = 'NC'
         AND SDESCAMPO = 'ORD_TIPPER';

      SELECT SVALOR
        INTO V_BEN_RELACION
        FROM LAFT.TBL_CONFIG_REPORTES
       WHERE SORIGEN = 'NC'
         AND SDESCAMPO = 'BEN_RELACION';

      SELECT SVALOR
        INTO V_BEN_CONDICION
        FROM LAFT.TBL_CONFIG_REPORTES
       WHERE SORIGEN = 'NC'
         AND SDESCAMPO = 'BEN_CONDICION';

      SELECT SVALOR
        INTO V_DAT_TIPFON
        FROM LAFT.TBL_CONFIG_REPORTES
       WHERE SORIGEN = 'NC'
         AND SDESCAMPO = 'DAT_TIPFON';

      SELECT SVALOR
        INTO V_DAT_TIPOPE
        FROM LAFT.TBL_CONFIG_REPORTES
       WHERE SORIGEN = 'NC'
         AND SDESCAMPO = 'DAT_TIPOPE';

      SELECT SVALOR
        INTO V_DAT_ALCANCE
        FROM LAFT.TBL_CONFIG_REPORTES
       WHERE SORIGEN = 'NC'
         AND SDESCAMPO = 'DAT_ALCANCE';

      SELECT SVALOR
        INTO V_DAT_FORMA
        FROM LAFT.TBL_CONFIG_REPORTES
       WHERE SORIGEN = 'NC'
         AND SDESCAMPO = 'DAT_FORMA';

      /*SELECT SVALOR INTO V_DATA_INTERM FROM LAFT.TBL_CONFIG_REPORTES
      WHERE SORIGEN='SIN' AND SDESCAMPO='DAT_INTOPE';*/

      V_SCLIENT_ORD := '01020517207331';

      SELECT /*+INDEX (AD XIF2110ADDRESS)*/
      --TRIM(CL.SCLIENAME),
      --TRIM(CI.SIDDOC),
       CL.SCLIENAME,
       CI.SIDDOC,
       ADC.STI_DIRE,
       ADC.SNOM_DIRECCION,
       ADC.SNUM_DIRECCION,
       ADC.STI_BLOCKCHALET,
       ADC.SBLOCKCHALET,
       ADC.STI_INTERIOR,
       ADC.SNUM_INTERIOR,
       ADC.STI_CJHT,
       ADC.SNOM_CJHT,
       ADC.SETAPA,
       ADC.SMANZANA,
       ADC.SLOTE,
       ADC.SREFERENCIA,
       NVL(AD.SSTREET, AD.SSTREET1), --TRIM(AD.SSTREET) || TRIM(AD.SSTREET1),
       ADC.SCLIENT,
       AD.NMUNICIPALITY,
       C.SDESCRIPT,
       D.SDESCRIPT,
       E.SDESCRIPT,
       CI.NIDDOC_TYPE,
       ISO.SCOD_COUNTRY_ISO

        INTO V_SCLIENAME_ORD,
             V_SIDDOC_ORD,
             V_STI_DIRE,
             V_SNOM_DIRECCION,
             V_SNUM_DIRECCION,
             V_STI_BLOCKCHALET,
             V_SBLOCKCHALET,
             V_STI_INTERIOR,
             V_SNUM_INTERIOR,
             V_STI_CJHT,
             V_SNOM_CJHT,
             V_SETAPA,
             V_SMANZANA,
             V_SLOTE,
             V_SREFERENCIA,
             V_SDIRECCION_ORD,
             V_SCLIENT_AD,
             V_NMUNICIPALITY_ORD,
             V_DEPARTAMENTO_ORD,
             V_DISTRITO_ORD,
             V_PROVINCIA_ORD,
             V_ORD_TIPDOC,
             V_ORD_PAIS

        FROM ADDRESS AD
        LEFT OUTER JOIN ADDRESS_CLIENT ADC
          ON ADC.SCLIENT = AD.SCLIENT
         AND ADC.NRECOWNER = AD.NRECOWNER
         AND ADC.SKEYADDRESS = AD.SKEYADDRESS
         AND ADC.DEFFECDATE = AD.DEFFECDATE
         AND ADC.SRECTYPE = AD.SRECTYPE
        LEFT OUTER JOIN CLIENT CL
          ON CL.SCLIENT = AD.SCLIENT
        LEFT OUTER JOIN CLIENT_IDDOC CI
          ON CI.SCLIENT = AD.SCLIENT
        LEFT OUTER JOIN PROVINCE C
          ON C.NPROVINCE = AD.NPROVINCE
        LEFT OUTER JOIN TAB_LOCAT D
          ON D.NLOCAL = AD.NLOCAL
        LEFT OUTER JOIN MUNICIPALITY E
          ON E.NMUNICIPALITY = AD.NMUNICIPALITY
        LEFT OUTER JOIN TBL_TRX_NATIONALITY_ISO ISO
          ON (NVL(CL.NNATIONALITY, AD.NCOUNTRY)) = ISO.NNATIONALITY
       WHERE AD.SCLIENT = V_SCLIENT_ORD
         AND AD.NRECOWNER = 2
         AND AD.SRECTYPE = 2
         AND AD.DNULLDATE IS NULL
         AND ISO.SACTIVE = '1'
         AND TRIM(AD.SKEYADDRESS) || TO_CHAR(AD.DCOMPDATE, 'YYYYMMDDHH24MMSS') || TO_CHAR(AD.DEFFECDATE, 'YYYYMMDDHH24MMSS') =
             (SELECT /*+INDEX (ADT XIF2110ADDRESS)*/
               MAX(TRIM(AT.SKEYADDRESS) || TO_CHAR(AT.DCOMPDATE, 'YYYYMMDDHH24MMSS') || TO_CHAR(AT.DEFFECDATE, 'YYYYMMDDHH24MMSS'))
                FROM ADDRESS AT
               WHERE AT.SCLIENT = AD.SCLIENT
                 AND AT.NRECOWNER = 2
                 AND AT.SRECTYPE = 2
                 AND AT.DNULLDATE IS NULL);
    EXCEPTION
      WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE(SQLERRM || CHR(13) || DBMS_UTILITY.FORMAT_ERROR_BACKTRACE || '  ---1');
        NULL;
    END;

    IF V_SCLIENT_AD IS NOT NULL THEN
      PKG_BDU_CLIENTE.SP_FORMA_FORMATDIRE(V_STI_DIRE,
                                          V_SNOM_DIRECCION,
                                          V_SNUM_DIRECCION,
                                          V_STI_BLOCKCHALET,
                                          V_SBLOCKCHALET,
                                          V_STI_INTERIOR,
                                          V_SNUM_INTERIOR,
                                          V_STI_CJHT,
                                          V_SNOM_CJHT,
                                          V_SETAPA,
                                          V_SMANZANA,
                                          V_SLOTE,
                                          V_SREFERENCIA,
                                          V_SDIRECCION_ORD);

    END IF;

    PKG_BDU_CLIENTE.SP_HOMOLDATOSOTROS('FIDELIZACION', 'RUBIGEO', V_NMUNICIPALITY_ORD, V_NMUNICIPALITY_ORD);

    --V_NPROVINCE_ORD := TRIM(TO_CHAR(V_NMUNICIPALITY_ORD, '000000'));
    --V_NLOCAL_ORD    := TRIM(TO_CHAR(V_NMUNICIPALITY_ORD, '000000'));

    BEGIN
      SELECT SPHONE
        INTO V_SPHONE_ORD
        FROM PHONES PH
       WHERE SUBSTR(PH.SKEYADDRESS, 2, 14) = V_SCLIENT_ORD
         AND PH.NRECOWNER = 2
         AND PH.DNULLDATE IS NULL
         AND TRIM(PH.SKEYADDRESS) || PH.NKEYPHONES || TO_CHAR(PH.DCOMPDATE, 'YYYYMMDDHH24MMSS') || TO_CHAR(PH.DEFFECDATE, 'YYYYMMDDHH24MMSS') =
             (SELECT /*+INDEX (PHT IDX_PHONE_1)*/
               MAX(TRIM(PHT.SKEYADDRESS) || PHT.NKEYPHONES || TO_CHAR(PHT.DCOMPDATE, 'YYYYMMDDHH24MMSS') || TO_CHAR(PHT.DEFFECDATE, 'YYYYMMDDHH24MMSS'))
                FROM PHONES PHT
               WHERE SUBSTR(PHT.SKEYADDRESS, 2, 14) = SUBSTR(PH.SKEYADDRESS, 2, 14)
                 AND PHT.NRECOWNER = 2
                 AND PHT.DNULLDATE IS NULL);

    EXCEPTION
      WHEN OTHERS THEN
        DBMS_OUTPUT.PUT_LINE(SQLERRM || CHR(13) || DBMS_UTILITY.FORMAT_ERROR_BACKTRACE || '  ---2');
        NULL;
    END;

    OPEN C_TABLE FOR
      SELECT TRIM(TO_CHAR(ROWNUM, '00000000')) AS FILA, --1-vb.801
             /*'0001'*/
             RPAD(DECODE(V_OFICINA,'',' ',V_OFICINA),4) AS OFICINA, --2
             TRIM(TO_CHAR(ROWNUM, '00000000')) AS OPERACION, --3-vb.802
             RPAD(TRIM( --TRIM(TO_CHAR(O.DCOMPDATE, 'YYYYMM')) ||
                       TRIM(TO_CHAR(DECODE(O.NRECEIPT, '', 0, TRIM(O.NRECEIPT)), '0000000000')) || TRIM(TO_CHAR(DECODE(O.NINTERTYP, '', 0, TRIM(O.NINTERTYP)), '00000'))),
                  20) AS INTERNO, --4
             O.MODALIDAD AS MODALIDAD, --5-vb.827
             /*'150141'*/
             V_OPE_UBIGEO AS OPE_UBIGEO, --6
             RPAD(DECODE(O.DCOMPDATE, '', ' ', TO_CHAR(O.DCOMPDATE, 'YYYYMMDD')), 8) AS OPE_FECHA, --7
             RPAD(DECODE(O.DCOMPDATE, '', ' ', TO_CHAR(O.DCOMPDATE, 'HH24MISS')), 6) AS OPE_HORA, --8
             RPAD(' ', 10) AS EJE_RELACION, --vb.835
             RPAD(' ', 1) AS EJE_CONDICION, --vb.836
             RPAD(' ', 1) AS EJE_TIPPER, --vb
             RPAD(' ', 1) AS EJE_TIPDOC, --vb
             RPAD(' ', 12) AS EJE_NUMDOC, --vb
             RPAD(' ', 11) AS EJE_NUMRUC, --vb
             RPAD(' ', 40) AS EJE_APEPAT, --vb
             RPAD(' ', 40) AS EJE_APEMAT, --vb
             RPAD(' ', 40) AS EJE_NOMBRES, --vb
             RPAD(' ', 4) AS EJE_OCUPACION, --vb
             --'' AS EJE_CIIU, --vb.845
             RPAD(' ', 6) AS EJE_PAIS,
             --'' AS EJE_DESCIIU, --vb.846
             RPAD(' ', 104) AS EJE_CARGO, --vb.847
             RPAD(' ', 2) AS EJE_PEP,
             RPAD(' ', 150) AS EJE_DOMICILIO, --vb.848
             RPAD(' ', 2) AS EJE_DEPART, --vb.849
             RPAD(' ', 2) AS EJE_PROV, --vb.850
             RPAD(' ', 2) AS EJE_DIST, --vb.851
             RPAD(' ', 40) AS EJE_TELEFONO, --vb.852

             RPAD( /*'1'*/ NVL(V_ORD_RELACION, ' '), 1) AS ORD_RELACION, --27-vb.735
             RPAD( /*'1'*/ NVL(V_ORD_CONDICION, ' '), 1) AS ORD_CONDICION, --28-vb.736
             RPAD(NVL(V_ORD_TIPPER, ' '), 1) AS ORD_TIPPER, --29-

             RPAD(CASE
                    WHEN V_ORD_TIPPER IN ('1', '2') THEN
                     NVL(V_ORD_TIPDOC, ' ')
                    ELSE
                     ' '
                  END

                 ,
                  1) AS ORD_TIPDOC, --30-vb.738
             RPAD(CASE
                    WHEN V_ORD_TIPPER IN ('1', '2') THEN
                     NVL(V_SIDDOC_ORD, ' ')
                    ELSE
                     ' '
                  END,
                  12) AS ORD_NUMDOC, --31-vb.671
             RPAD(CASE
                    WHEN V_ORD_TIPPER IN (3, 4) THEN
                     NVL(V_SIDDOC_ORD, ' ')
                    ELSE
                     ' '
                  END,
                  11) AS ORD_NUMRUC,
             --RPAD(DECODE(DECODE(RIGHT(V_SIDDOC_ORD, 11), '', ' ', RIGHT(V_SIDDOC_ORD, 11)), '', ' '), 11) AS ORD_NUMRUC, --32-vb.672
             RPAD(NVL(V_SCLIENAME_ORD, ' '), 120) AS ORD_APEPAT, --33
             RPAD(' ', 40) AS ORD_APEMAT, --34
             RPAD(' ', 40) AS ORD_NOMBRES, --35
             RPAD(' ', 4) AS ORD_OCUPACION, --36-vb.744
             RPAD(DECODE(V_ORD_PAIS, '', ' ', V_ORD_PAIS), 6) AS ORD_PAIS, --37
             --RPAD('' AS Ord_CIIU,--37-YA NO SE SOLICITA
             --RPAD('' AS Ord_DesCIIU,--38-YA NO SE SOLICITA
             RPAD(' ', 104) AS ORD_CARGO, --38-NO ES OBLIGATORIO.vb747
             RPAD(' ', 2) AS ORD_PEP, --39-NO ES OBLIGATORIO
             RPAD(

             REPLACE(REPLACE(REPLACE(

             UPPER(DECODE(V_SDIRECCION_ORD, '', ' ', TRIM(V_SDIRECCION_ORD)) || ' ' || DECODE(V_DISTRITO_ORD, '', ' ', TRIM(V_DISTRITO_ORD)) || ' ' ||
                  DECODE(V_PROVINCIA_ORD, '', ' ', TRIM(V_PROVINCIA_ORD)) || ' ' || DECODE(V_DEPARTAMENTO_ORD, '', ' ', TRIM(V_DEPARTAMENTO_ORD)))

            , '?', '#'),
                            'Ñ',
                            '#'),
                    'ñ',
                    '#')
                  ,50) AS ORD_DOMICILIO, --40
             RPAD(DECODE(V_NMUNICIPALITY_ORD, '', '15', SUBSTR(V_NMUNICIPALITY_ORD, 1, 2)), 2) AS ORD_DEPART, --41
             RPAD(DECODE(V_NMUNICIPALITY_ORD, '', '01', SUBSTR(V_NMUNICIPALITY_ORD, 3, 2)), 2) AS ORD_PROV, --42
             RPAD(DECODE(V_NMUNICIPALITY_ORD, '', '41', SUBSTR(V_NMUNICIPALITY_ORD, 5, 2)), 2) AS ORD_DIST, --43
             --V_NMUNICIPALITY_ORD AS ORD_UBIGEO,
             RPAD(NVL(V_SPHONE_ORD, ' '), 40) AS ORD_TELEFONO, --44

             RPAD('1', 1) AS BEN_RELACION, --45
             RPAD('1', 1) AS BEN_CONDICION, --46
             --RPAD(DECODE(SUBSTR(clBENEF.SCLIENT, 1, 2), '02', '2', '01', '3', ' '), 1) AS BEN_TIP_PER, --47
             RPAD(NVL(CL.TIPO_PERSONA, ' '), 1) AS BEN_TIP_PER,
             RPAD(NVL(CL.TIPO_DOCUMENTO, ' '), 1) AS BEN_TIP_DOC,
             RPAD(CASE
                    WHEN CL.TIPO_PERSONA IN ('1', '2') THEN
                     CL.NUM_DOC
                    ELSE
                     ' '
                  END,
                  12) AS BEN_NUM_DOC,
             RPAD(CASE
                    WHEN CL.TIPO_PERSONA IN ('3', '4') THEN
                     RIGHT(CL.SCLIENT, 11)
                    ELSE
                     ' '
                  END,
                  12) AS BEN_NUM_RUC,
             RPAD(REPLACE(REPLACE(REPLACE(CASE
                                            WHEN CL.TIPO_PERSONA IN ('1', '2') THEN
                                             NVL(CL.SLASTNAME, ' ')
                                            WHEN CL.TIPO_PERSONA IN ('3', '4') THEN
                                             NVL(CL.SCLIENAME, ' ')
                                            ELSE
                                             ' '
                                          END,
                                          '?',
                                          '#'),
                                  'Ñ',
                                  '#'),
                          'ñ',
                          '#'),
                  120) AS BEN_APEPAT,
             --RPAD(REPLACE(REPLACE(REPLACE(DECODE(SUBSTR(CLORDEN.SCLIENT, 1, 2), '02', CLORDEN.SLASTNAME, '01', CLORDEN.SCLIENAME, ' '), '?', '#'), 'Ñ', '#'), 'ñ', '#'), 120) AS BEN_APEPAT, --51
             RPAD(REPLACE(REPLACE(REPLACE(DECODE(CL.SLASTNAME2, '', ' ', CL.SLASTNAME2), '?', '#'), 'Ñ', '#'), 'ñ', '#'), 40) AS BEN_APEMAT, --52
             RPAD(REPLACE(REPLACE(REPLACE(DECODE(CL.SFIRSTNAME, '', ' ', CL.SFIRSTNAME), '?', '#'), 'Ñ', '#'), 'ñ', '#'), 40) AS BEN_NOMBRES, --53

             RPAD(NVL(CL.COD_ESPECIALIDAD_SBS, ' '), 4) AS BEN_OCUPACION, --54
             RPAD(DECODE(CL.SCOD_COUNTRY_ISO, '', ' ', CL.SCOD_COUNTRY_ISO), 6) AS BEN_PAIS,
             RPAD(' ', 104) AS BEN_CARGO, --56-NO ES OBLIGATORIO
             RPAD(' ', 2) AS BEN_PEP, --57-NO ES OBLIGATORIO
             RPAD(REPLACE(REPLACE(REPLACE(UPPER(DECODE(CL.SSTREET, '', ' ', TRIM(CL.SSTREET)) || ' ' || TRIM(CL.DISTRITO) || ' ' || TRIM(CL.PROV) || ' ' || TRIM(CL.DEPART)), '?', '#'), 'Ñ', '#'), 'ñ', '#'),
                  150) AS BEN_DOMICILIO, --58
             --TO_NUMBER(CLORDEN.COD_UBI_CLI) AS BEN_UBIGEO,
             RPAD(DECODE(CL.NMUNICIPALITY, '', '15', SUBSTR(TRIM(TO_CHAR((CL.COD_UBI_CLI), '000000')), 1, 2)), 2) AS BEN_DEPART, --59
             RPAD(DECODE(CL.NMUNICIPALITY, '', '01', SUBSTR(TRIM(TO_CHAR((CL.COD_UBI_CLI), '000000')), 3, 2)), 2) AS BEN_PROV, --60
             RPAD(DECODE(CL.NMUNICIPALITY, '', '41', SUBSTR(TRIM(TO_CHAR((CL.COD_UBI_CLI), '000000')), 5, 2)), 2) AS BEN_DIST, --61
             RPAD(DECODE(CL.SPHONE, '', ' ', CL.SPHONE), 40) AS BEN_TELEFONO, --62
             RPAD( /*'2'*/ V_DAT_TIPFON, 1) AS DAT_TIPFON, --63
             RPAD('54' /*V_DAT_TIPOPE*/, 2) AS DAT_TIPOPE, --64
             RPAD(' ', 40) AS DAT_DESOPE, --65
             RPAD(' ', 80) AS DAT_ORIFON, --66
             RPAD(DECODE(O.NCURRENCY, '1', 'PEN', 'USD'), 3) AS DAT_MONOPE, --67
             RPAD(' ', 3) AS DAT_MONOPE_A, --68
             RPAD(TO_CHAR(CAST((O.NAMOUNT) AS NUMBER(15, 2)), '000000000000000.00'), 29) AS DAT_MTOOPE, --MTO_PENSIONGAR , MTO_PRIUNI 69
             RPAD(' ', 30) AS DAT_MTOOPEA, --MTO_PENSIONGAR , MTO_PRIUNI 70
             RPAD(' ', 5) AS DAT_COD_ENT_INVO, --71--NO OBLIGATORIO
             RPAD('1', 1) AS DAT_COD_TIP_CTAO, --72--NO OBLIGATORIO
             RPAD('002193116232365', 20) AS DAT_COD_CTAO, --73--NO OBLIGATORIO
             RPAD(' ', 150) AS DAT_ENT_FNC_EXTO, --74--NO OBLIGATORIO
             RPAD(' ', 5) AS DAT_COD_ENT_INVB, --75--NO OBLIGATORIO
             RPAD('1', 1) AS DAT_COD_TIP_CTAB, --76--NO OBLIGATORIO
             RPAD(' ', 20) AS DAT_COD_CTAB, --77--NO OBLIGATORIO
             RPAD(' ', 150) AS DAT_ENT_FNC_EXTB, --78--NO OBLIGATORIO
             RPAD( /*'1'*/ V_DAT_ALCANCE, 1) AS DAT_ALCANCE, --79
             RPAD(' ', 2) AS DAT_COD_PAISO, --80--ALCANCE 1, ESTE CAMPO VA EN BLANCO
             RPAD(' ', 2) AS DAT_COD_PAISD, --81--ALCANCE 1, ESTE CAMPO VA EN BLANCO
             RPAD('2', 1) AS DAT_INTOPE, --82--NO OBLIGATORIO
             RPAD( /*'2'*/ V_DAT_FORMA, 1) AS DAT_FORMA, --83
             RPAD(' ', 40) AS DAT_INFORM, --84--NO OBLIGATORIO
             O.TIPO_INTERMEDIARIO AS TIPO_INTERM

        FROM (SELECT P.NBRANCH,
                      P.NPRODUCT,
                      P.NPOLICY,
                      P.NCERTIF,
                      P.NRECEIPT,
                      P.NPERCENT,
                      P.NAMOUNT,
                      P.DCOMPDATE,
                      I.SCLIENT,
                      PR.NCURRENCY,
                      'U' AS MODALIDAD,
                      IT.SDESCRIPT AS TIPO_INTERMEDIARIO,
                      P.NINTERTYP
                 FROM PV_COMMISSION_FLOW P
                 LEFT OUTER JOIN INTERMEDIA I
                   ON I.NINTERMED = P.NINTERMED
                 LEFT OUTER JOIN PREMIUM PR
                   ON PR.NRECEIPT = P.NRECEIPT
                 LEFT OUTER JOIN INTERM_TYP IT
                   ON IT.NINTERTYP = P.NINTERTYP
                WHERE --P.NINTERTYP = 3
                DECODE(PR.NCURRENCY, 2, P.NAMOUNT, ROUND(P.NAMOUNT / P_TC, 4)) >= P_MONTO
               --AND TO_CHAR(TRUNC(P.DCOMPDATE),'DD/MM/YYYY') BETWEEN '01/01/2018' AND '10/01/2020'
             AND TO_CHAR(TRUNC(P.DCOMPDATE), 'DD/MM/YYYY') BETWEEN P_FECINI AND P_FECFIN) O
        LEFT OUTER JOIN (SELECT C.SCLIENAME,
                                C.SFIRSTNAME,
                                C.SLASTNAME,
                                C.SLASTNAME2,
                                C.NQ_CHILD,
                                ADR.SSTREET,
                                DPTO.SDESCRIPT AS DEPART,
                                PROV.SDESCRIPT AS PROV,
                                DIST.SDESCRIPT AS DISTRITO,
                                PH.SPHONE,
                                C.STAX_CODE,
                                DIST.NMUNICIPALITY,
                                C.SCLIENT,
                                EQUI.COD_UBI_CLI,
                                DCLI.NIDDOC_TYPE AS TIP_DOC,
                                DCLI.SIDDOC AS NUM_DOC,
                                C.NNATIONALITY,
                                NVL(NI.SCOD_COUNTRY_ISO, '0999'),
                                CASE
                                  WHEN SUBSTR(C.SCLIENT, 1, 2) = '01' THEN
                                   CASE
                                     WHEN SUBSTR(C.SCLIENT, 4, 2) = '20' THEN
                                      '3' --'Persona juridica'
                                     ELSE
                                      '1' --'persona natural con negocio'
                                   END
                                  ELSE
                                   '1' --'Persona natural'
                                END AS TIPO_PERSONA,
                                CASE
                                  WHEN DCLI.NIDDOC_TYPE = 2 THEN
                                   '1'
                                  WHEN DCLI.NIDDOC_TYPE = 4 THEN
                                   '2'
                                  WHEN DCLI.NIDDOC_TYPE = 6 THEN
                                   '5'
                                  WHEN DCLI.NIDDOC_TYPE = 1 THEN
                                   ' '
                                  WHEN DCLI.NIDDOC_TYPE IN (3, 5, 7, 8, 9, 10, 11, 0, 12, 13) THEN
                                   '9'
                                  ELSE
                                   ' '
                                END AS TIPO_DOCUMENTO,
                                COS.NIDOCUP_SBS AS COD_ESPECIALIDAD_SBS,
                                NI.SCOD_COUNTRY_ISO
                           FROM CLIENT C
                           LEFT OUTER JOIN (SELECT SCLIENT,
                                                  NLOCAL,
                                                  NPROVINCE,
                                                  NMUNICIPALITY,
                                                  NCOUNTRY,
                                                  SKEYADDRESS,
                                                  SSTREET
                                             FROM ADDRESS ADRR
                                            WHERE ADRR.NRECOWNER = 2
                                              AND ADRR.SRECTYPE = 2
                                              AND ADRR.DNULLDATE IS NULL
                                              AND TRIM(ADRR.SKEYADDRESS) || TO_CHAR(ADRR.DCOMPDATE, 'YYYYMMDDHH24MMSS') || TO_CHAR(ADRR.DEFFECDATE, 'YYYYMMDDHH24MMSS') =
                                                  (SELECT /*+INDEX (AT XIF2110ADDRESS)*/
                                                    MAX(TRIM(AT.SKEYADDRESS) || TO_CHAR(AT.DCOMPDATE, 'YYYYMMDDHH24MMSS') || TO_CHAR(AT.DEFFECDATE, 'YYYYMMDDHH24MMSS'))
                                                     FROM ADDRESS AT
                                                    WHERE AT.SCLIENT = ADRR.SCLIENT
                                                      AND AT.NRECOWNER = 2
                                                      AND AT.SRECTYPE = 2
                                                      AND AT.DNULLDATE IS NULL)) ADR
                             ON ADR.SCLIENT = C.SCLIENT
                           LEFT OUTER JOIN PROVINCE DPTO
                             ON DPTO.NPROVINCE = ADR.NPROVINCE
                           LEFT OUTER JOIN TAB_LOCAT PROV
                             ON PROV.NLOCAL = ADR.NLOCAL
                           LEFT OUTER JOIN MUNICIPALITY DIST
                             ON DIST.NMUNICIPALITY = ADR.NMUNICIPALITY
                           LEFT OUTER JOIN (SELECT
                                           /*+INDEX (PHR IDX_PHONE_1)*/
                                            SPHONE,
                                            NKEYPHONES,
                                            SKEYADDRESS,
                                            DCOMPDATE,
                                            DEFFECDATE
                                             FROM PHONES PHR
                                            WHERE PHR.NRECOWNER = 2
                                              AND PHR.DNULLDATE IS NULL
                                              AND TRIM(PHR.SKEYADDRESS) || PHR.NKEYPHONES || TO_CHAR(PHR.DCOMPDATE, 'YYYYMMDDHH24MMSS') || TO_CHAR(PHR.DEFFECDATE, 'YYYYMMDDHH24MMSS') =
                                                  (SELECT /*+INDEX (PHT IDX_PHONE_1)*/
                                                    MAX(TRIM(PHT.SKEYADDRESS) || PHT.NKEYPHONES || TO_CHAR(PHT.DCOMPDATE, 'YYYYMMDDHH24MMSS') || TO_CHAR(PHT.DEFFECDATE, 'YYYYMMDDHH24MMSS'))
                                                     FROM PHONES PHT
                                                    WHERE SUBSTR(PHT.SKEYADDRESS, 2, 14) = SUBSTR(PHR.SKEYADDRESS, 2, 14)
                                                      AND PHT.NRECOWNER = 2
                                                      AND PHT.DNULLDATE IS NULL)) PH
                             ON SUBSTR(PH.SKEYADDRESS, 2, 14) = C.SCLIENT
                           LEFT OUTER JOIN EQUI_UBIGEO EQUI
                             ON TO_NUMBER(EQUI.COD_UBI_DIS) = DIST.NMUNICIPALITY
                            AND EQUI.COD_CLI = '11111111111111'
                           LEFT OUTER JOIN TBL_TRX_NATIONALITY_ISO NI
                             ON C.NNATIONALITY = NI.NNATIONALITY
                            AND NI.SACTIVE = 1
                           LEFT OUTER JOIN CLIENT_IDDOC DCLI
                             ON DCLI.SCLIENT = C.SCLIENT
                           LEFT OUTER JOIN EQUI_UBIGEO EQUI
                             ON TO_NUMBER(EQUI.COD_UBI_DIS) = DIST.NMUNICIPALITY
                            AND EQUI.COD_CLI = '11111111111111'
                           LEFT OUTER JOIN TBL_TRX_NATIONALITY_ISO NI
                             ON C.NNATIONALITY = NI.NNATIONALITY
                            AND NI.SACTIVE = 1
                           LEFT OUTER JOIN TBL_CONFIG_OCUP_SBS COS
                             ON COS.NIDOCUPACION = C.NSPECIALITY
                            AND COS.SORIGEN_BD = 'TIME'
                            AND COS.SACTIVE = '1') CL
          ON CL.SCLIENT = O.SCLIENT;

  END COMISIONES;

  PROCEDURE SPS_OBT_CORREO_ENVIO(P_USUARIO  NUMBER,
                                 P_SEMAIL   OUT VARCHAR2,
                                 P_SNAME    OUT VARCHAR2,
                                 P_NCODE    OUT NUMBER,
                                 P_SMESSAGE OUT VARCHAR2) AS
  BEGIN
    P_NCODE := 0;
    BEGIN
      SELECT SEMAIL,
             SNAME
        INTO P_SEMAIL,
             P_SNAME
        FROM PRO_USERS_PRO@DBL_TIME_PRTCANAL.PROTECTA.DOM
       WHERE NIDUSER = P_USUARIO;

    EXCEPTION
      WHEN NO_DATA_FOUND THEN
        P_SEMAIL   := '';
        P_NCODE    := 1;
        P_SMESSAGE := 'No existe el usuario o se encuentra inactivo';
      WHEN OTHERS THEN
        P_SEMAIL   := '';
        P_NCODE    := 1;
        P_SMESSAGE := SQLERRM || '  - Comuníquese con el administrador' || CHR(13) || DBMS_UTILITY.FORMAT_ERROR_BACKTRACE;
    END;
  END;
/*
  PROCEDURE GENERAR_REP_SBS(P_OPE          NUMBER,
                            P_TC           NUMBER,
                            P_MONTO        NUMBER,
                            P_FECINI       VARCHAR2,
                            P_FECFIN       VARCHAR2,
                            P_EST_REPORTES VARCHAR2,
                            P_TIPO_ARCHIVO VARCHAR2,
                            P_SRUTA        OUT VARCHAR2,
                            P_ID_REPORTE   OUT VARCHAR2,
                            P_NCODE        OUT NUMBER,
                            P_SMESSAGE     OUT VARCHAR2,
                            C_TABLE        OUT SYS_REFCURSOR) IS

    V_SSEPARADOR_CSV CHAR(1) := ';';
    V_SSEPARADOR_TXT CHAR(1) := '|';
    V_TIPO_ARCHIVO   VARCHAR2(10);
    V_OPE_UNI        NUMBER(10);
    V_OPE_MUL        NUMBER(10);
    WTEXT            VARCHAR2(30000);
    WNOMARCH         VARCHAR2(255);
    WRUTA            VARCHAR2(255);
    FILE_OUTPUT      UTL_FILE.FILE_TYPE;
    --V_C_TABLE        MYCURSOR2;
    V_C_TABLE MYCURSOR;

    V_SID VARCHAR2(100);

  BEGIN
    P_NCODE    := 0;
    P_SMESSAGE := 'El proceso de generación del reporte SBS culminó exitosamente';

    V_SID := 'SBS_' || TO_CHAR(SYSTIMESTAMP, 'yyyymmddHH24MISSFF2') || '.' || P_TIPO_ARCHIVO;
    --V_TIPO_CSV :=
    --V_TIPO_TXT :=

    P_ID_REPORTE := V_SID;

    --PERSONALIZACIÓN DEL ARCHIVO
    WNOMARCH := 'REPORTE_' || 'SBS_' || TO_CHAR(SYSTIMESTAMP, 'yyyymmddHH24MISSFF2') || '.' || P_TIPO_ARCHIVO;

    --UBICACIÓN DE LA CARPETA
    SELECT MAX(SDATA)
      INTO WRUTA
      FROM TBL_PD_SCTR_CONFIG
     WHERE STYPE = 'DIRECTORIO_GEN_TRAMA';

    --WRUTA := 'SBS';

    --UBICACIÓN DEL SERVIDOR
    SELECT MAX(SDATA)
      INTO P_SRUTA
      FROM TBL_PD_SCTR_CONFIG
     WHERE STYPE = 'IP_SERVER_GEN_TRAMA';

    --EMPEZAMOS A CONSTRUIR EL ARCHIVO
    FILE_OUTPUT := UTL_FILE.FOPEN(WRUTA, WNOMARCH, 'w', 30000);

    INSERT INTO LAFT.TBL_TRX_MONITOREO_REP_SBS
      (SID,
       DINIREP,
       DFINREP,
       NTIPCAMBIO,
       STIPOPE,
       NMONTO,
       NUSERCODE,
       NSTATUSPROC,
       DCOMPDATE,
       DINIPROC,
       DFINPROC,
       SORIGEN,
       STIPOARCH)
    VALUES
      (V_SID,
       P_FECINI,
       P_FECFIN,
       P_TC,
       P_OPE,
       P_MONTO,
       '999',
       1,
       SYSDATE,
       SYSDATE,
       NULL,
       P_EST_REPORTES,
       P_TIPO_ARCHIVO);

    --CONSTRUIMOS EL ARCHIVO EXCEL:
    IF P_TIPO_ARCHIVO = 'csv' THEN

      --CABECERA PARA CSV
      WTEXT := '0501' || V_SSEPARADOR_CSV || '01' || V_SSEPARADOR_CSV || 'REVIS' || V_SSEPARADOR_CSV || TO_CHAR(SYSDATE, 'YYYYMMDD') || V_SSEPARADOR_CSV || '012' || V_SSEPARADOR_CSV ||
               '               ';

      --TERMINA LA LÍNEA DE CABECERA
      UTL_FILE.PUT_LINE(FILE_OUTPUT, WTEXT);

      IF P_EST_REPORTES = 'S' THEN

        SINIESTROS_MAYORES(P_OPE, P_TC, P_MONTO, P_FECINI, P_FECFIN, V_C_TABLE);

        WTEXT := '';
        SETEAR_VARIABLES;

        LOOP
          FETCH V_C_TABLE
            INTO V_FILA,
                 V_OFICINA,
                 V_OPERACION,
                 V_INTERNO,
                 V_MODALIDAD,
                 V_OPE_UBIGEO,
                 V_OPE_FECHA,
                 V_OPE_HORA,
                 V_EJE_RELACION,
                 V_EJE_CONDICION,
                 V_EJE_TIPPER,
                 V_EJE_TIPDOC,
                 V_EJE_NUMDOC,
                 V_EJE_NUMRUC,
                 V_EJE_APEPAT,
                 V_EJE_APEMAT,
                 V_EJE_NOMBRES,
                 V_EJE_OCUPACION,
                 V_EJE_PAIS,
                 V_EJE_CARGO,
                 V_EJE_PEP,
                 V_EJE_DOMICILIO,
                 V_EJE_DEPART,
                 V_EJE_PROV,
                 V_EJE_DIST,
                 V_EJE_TELEFONO,
                 V_ORD_RELACION,
                 V_ORD_CONDICION,
                 V_ORD_TIPPER,
                 V_ORD_TIPDOC,
                 V_ORD_NUMDOC,
                 V_ORD_NUMRUC,
                 V_ORD_APEPAT,
                 V_ORD_APEMAT,
                 V_ORD_NOMBRES,
                 V_ORD_OCUPACION,
                 V_ORD_PAIS,
                 V_ORD_CARGO,
                 V_ORD_PEP,
                 V_ORD_DOMICILIO,
                 V_ORD_DEPART,
                 V_ORD_PROV,
                 V_ORD_DIST,
                 --
                 V_ORD_TELEFONO,
                 V_BEN_RELACION,
                 V_BEN_CONDICION,
                 V_BEN_TIP_PER,
                 V_BEN_TIP_DOC,
                 V_BEN_NUM_DOC,
                 V_BEN_NUM_RUC,
                 V_BEN_APEPAT,
                 V_BEN_APEMAT,
                 V_BEN_NOMBRES,
                 V_BEN_OCUPACION,
                 V_BEN_PAIS,
                 V_BEN_CARGO,
                 V_BEN_PEP,
                 V_BEN_DOMICILIO,
                 V_BEN_DEPART,
                 V_BEN_PROV,
                 V_BEN_DIST,
                 V_BEN_TELEFONO,
                 V_DAT_TIPFON,
                 V_DAT_TIPOPE,
                 V_DAT_DESOPE,
                 V_DAT_ORIFON,
                 V_DAT_MONOPE,
                 V_DAT_MONOPE_A,
                 V_DAT_MTOOPE,
                 V_DAT_MTOOPEA,
                 V_DAT_COD_ENT_INVO,
                 V_DAT_COD_TIP_CTAO,
                 V_DAT_COD_CTAO,
                 V_DAT_ENT_FNC_EXTO,
                 V_DAT_COD_ENT_INVB,
                 V_DAT_COD_TIP_CTAB,
                 V_DAT_COD_CTAB,
                 V_DAT_ENT_FNC_EXTB,
                 V_DAT_ALCANCE,
                 V_DAT_COD_PAISO,
                 V_DAT_COD_PAISD,
                 V_DAT_INTOPE,
                 V_DAT_FORMA,
                 V_DAT_INFORM,
                 V_ORIGEN;

          EXIT WHEN V_C_TABLE%NOTFOUND;

          --CUERPO DEL ARCHIVO
          WTEXT := V_FILA || V_SSEPARADOR_CSV || V_OFICINA || V_SSEPARADOR_CSV || V_OPERACION || V_SSEPARADOR_CSV || V_INTERNO || V_SSEPARADOR_CSV || V_MODALIDAD || V_SSEPARADOR_CSV || V_OPE_UBIGEO ||
                   V_SSEPARADOR_CSV || V_OPE_FECHA || V_SSEPARADOR_CSV || V_OPE_HORA || V_SSEPARADOR_CSV || V_EJE_RELACION || V_SSEPARADOR_CSV || V_EJE_CONDICION || V_SSEPARADOR_CSV || V_EJE_TIPPER ||
                   V_SSEPARADOR_CSV || V_EJE_TIPDOC || V_SSEPARADOR_CSV || V_EJE_NUMDOC || V_SSEPARADOR_CSV || V_EJE_NUMRUC || V_SSEPARADOR_CSV || V_EJE_APEPAT || V_SSEPARADOR_CSV || V_EJE_APEMAT ||
                   V_SSEPARADOR_CSV || V_EJE_NOMBRES || V_SSEPARADOR_CSV || V_EJE_OCUPACION || V_SSEPARADOR_CSV || V_EJE_PAIS || V_SSEPARADOR_CSV || V_EJE_CARGO || V_SSEPARADOR_CSV || V_EJE_PEP ||
                   V_SSEPARADOR_CSV || V_EJE_DOMICILIO || V_SSEPARADOR_CSV || V_EJE_DEPART || V_SSEPARADOR_CSV || V_EJE_PROV || V_SSEPARADOR_CSV || V_EJE_DIST || V_SSEPARADOR_CSV || V_EJE_TELEFONO ||
                   V_SSEPARADOR_CSV || V_ORD_RELACION || V_SSEPARADOR_CSV || V_ORD_CONDICION || V_SSEPARADOR_CSV || V_ORD_TIPPER || V_SSEPARADOR_CSV || V_ORD_TIPDOC || V_SSEPARADOR_CSV ||
                   V_ORD_NUMDOC || V_SSEPARADOR_CSV || V_ORD_NUMRUC || V_SSEPARADOR_CSV || V_ORD_APEPAT || V_SSEPARADOR_CSV || V_ORD_APEMAT || V_SSEPARADOR_CSV || V_ORD_NOMBRES || V_SSEPARADOR_CSV ||
                   V_ORD_OCUPACION || V_SSEPARADOR_CSV || V_ORD_PAIS || V_SSEPARADOR_CSV || V_ORD_CARGO || V_SSEPARADOR_CSV || V_ORD_PEP || V_SSEPARADOR_CSV || V_ORD_DOMICILIO || V_SSEPARADOR_CSV ||
                   V_ORD_DEPART || V_SSEPARADOR_CSV || V_ORD_PROV || V_SSEPARADOR_CSV || V_ORD_DIST || V_SSEPARADOR_CSV || V_ORD_TELEFONO || V_SSEPARADOR_CSV || V_BEN_RELACION || V_SSEPARADOR_CSV ||
                   V_BEN_CONDICION || V_SSEPARADOR_CSV || V_BEN_TIP_PER || V_SSEPARADOR_CSV || V_BEN_TIP_DOC || V_SSEPARADOR_CSV || V_BEN_NUM_DOC || V_SSEPARADOR_CSV || V_BEN_NUM_RUC ||
                   V_SSEPARADOR_CSV || V_BEN_APEPAT || V_SSEPARADOR_CSV || V_BEN_APEMAT || V_SSEPARADOR_CSV || V_BEN_NOMBRES || V_SSEPARADOR_CSV || V_BEN_OCUPACION || V_SSEPARADOR_CSV || V_BEN_PAIS ||
                   V_SSEPARADOR_CSV || V_BEN_CARGO || V_SSEPARADOR_CSV || V_BEN_PEP || V_SSEPARADOR_CSV || V_BEN_DOMICILIO || V_SSEPARADOR_CSV || V_SSEPARADOR_CSV || V_BEN_DEPART || V_SSEPARADOR_CSV ||
                   V_BEN_PROV || V_SSEPARADOR_CSV || V_BEN_DIST || V_SSEPARADOR_CSV || V_BEN_TELEFONO || V_SSEPARADOR_CSV || V_DAT_TIPFON || V_SSEPARADOR_CSV || V_DAT_TIPOPE || V_SSEPARADOR_CSV ||
                   V_DAT_DESOPE || V_SSEPARADOR_CSV || V_DAT_ORIFON || V_SSEPARADOR_CSV || V_DAT_MONOPE || V_SSEPARADOR_CSV || V_DAT_MONOPE_A || V_SSEPARADOR_CSV || V_DAT_MTOOPE || V_SSEPARADOR_CSV ||
                   V_DAT_MTOOPEA || V_SSEPARADOR_CSV || V_DAT_COD_ENT_INVO || V_SSEPARADOR_CSV || V_DAT_COD_TIP_CTAO || V_SSEPARADOR_CSV || V_DAT_COD_CTAO || V_SSEPARADOR_CSV || V_DAT_ENT_FNC_EXTO ||
                   V_SSEPARADOR_CSV || V_DAT_COD_ENT_INVB || V_SSEPARADOR_CSV || V_DAT_COD_TIP_CTAB || V_SSEPARADOR_CSV || V_DAT_COD_CTAB || V_SSEPARADOR_CSV || V_DAT_ENT_FNC_EXTB || V_SSEPARADOR_CSV ||
                   V_DAT_ALCANCE || V_SSEPARADOR_CSV || V_DAT_COD_PAISO || V_SSEPARADOR_CSV || V_DAT_COD_PAISD || V_SSEPARADOR_CSV || V_DAT_INTOPE || V_SSEPARADOR_CSV || V_DAT_FORMA ||
                   V_SSEPARADOR_CSV || V_DAT_INFORM || V_SSEPARADOR_CSV || V_ORIGEN;

          UTL_FILE.PUT_LINE(FILE_OUTPUT, WTEXT);
        END LOOP;

      ELSIF P_EST_REPORTES = 'V' THEN

        VIDA_MAYORES(P_OPE, P_TC, P_MONTO, P_FECINI, P_FECFIN, V_C_TABLE);

        WTEXT := '';
        SETEAR_VARIABLES;

        LOOP
          FETCH V_C_TABLE
            INTO V_FILA,
                 V_OFICINA,
                 V_OPERACION,
                 V_INTERNO,
                 V_MODALIDAD,
                 V_OPE_UBIGEO,
                 V_OPE_FECHA,
                 V_OPE_HORA,
                 V_EJE_RELACION,
                 V_EJE_CONDICION,
                 V_EJE_TIPPER,
                 V_EJE_TIPDOC,
                 V_EJE_NUMDOC,
                 V_EJE_NUMRUC,
                 V_EJE_APEPAT,
                 V_EJE_APEMAT,
                 V_EJE_NOMBRES,
                 V_EJE_OCUPACION,
                 V_EJE_PAIS,
                 V_EJE_CARGO,
                 V_EJE_PEP,
                 V_EJE_DOMICILIO,
                 V_EJE_DEPART,
                 V_EJE_PROV,
                 V_EJE_DIST,
                 V_EJE_TELEFONO,
                 V_ORD_RELACION,
                 V_ORD_CONDICION,
                 V_ORD_TIPPER,
                 V_ORD_TIPDOC,
                 V_ORD_NUMDOC,
                 V_ORD_NUMRUC,
                 V_ORD_APEPAT,
                 V_ORD_APEMAT,
                 V_ORD_NOMBRES,
                 V_ORD_OCUPACION,
                 V_ORD_PAIS,
                 V_ORD_CARGO,
                 V_ORD_PEP,
                 V_ORD_DOMICILIO,
                 V_ORD_DEPART,
                 V_ORD_PROV,
                 V_ORD_DIST,
                 V_ORD_TELEFONO,
                 V_BEN_RELACION,
                 V_BEN_CONDICION,
                 V_BEN_TIP_PER,
                 V_BEN_TIP_DOC,
                 V_BEN_NUM_DOC,
                 V_BEN_NUM_RUC,
                 V_BEN_APEPAT,
                 V_BEN_APEMAT,
                 V_BEN_NOMBRES,
                 V_BEN_OCUPACION,
                 V_BEN_PAIS,
                 V_BEN_CARGO,
                 V_BEN_PEP,
                 V_BEN_DOMICILIO,
                 V_BEN_DEPART,
                 V_BEN_PROV,
                 V_BEN_DIST,
                 V_BEN_TELEFONO,
                 V_DAT_TIPFON,
                 V_DAT_TIPOPE,
                 V_DAT_DESOPE,
                 V_DAT_ORIFON,
                 V_DAT_MONOPE,
                 V_DAT_MONOPE_A,
                 V_DAT_MTOOPE,
                 V_DAT_MTOOPEA,
                 V_DAT_COD_ENT_INVO,
                 V_DAT_COD_TIP_CTAO,
                 V_DAT_COD_CTAO,
                 V_DAT_ENT_FNC_EXTO,
                 V_DAT_COD_ENT_INVB,
                 V_DAT_COD_TIP_CTAB,
                 V_DAT_COD_CTAB,
                 V_DAT_ENT_FNC_EXTB,
                 V_DAT_ALCANCE,
                 V_DAT_COD_PAISO,
                 V_DAT_COD_PAISD,
                 V_DAT_INTOPE,
                 V_DAT_FORMA,
                 V_DAT_INFORM,
                 V_ORIGEN;

          EXIT WHEN V_C_TABLE%NOTFOUND;

          --CUERPO DEL ARCHIVO
          WTEXT := V_FILA || V_SSEPARADOR_CSV || V_OFICINA || V_SSEPARADOR_CSV || V_OPERACION || V_SSEPARADOR_CSV || V_INTERNO || V_SSEPARADOR_CSV || V_MODALIDAD || V_SSEPARADOR_CSV || V_OPE_UBIGEO ||
                   V_SSEPARADOR_CSV || V_OPE_FECHA || V_SSEPARADOR_CSV || V_OPE_HORA || V_SSEPARADOR_CSV || V_EJE_RELACION || V_SSEPARADOR_CSV || V_EJE_CONDICION || V_SSEPARADOR_CSV || V_EJE_TIPPER ||
                   V_SSEPARADOR_CSV || V_EJE_TIPDOC || V_SSEPARADOR_CSV || V_EJE_NUMDOC || V_SSEPARADOR_CSV || V_EJE_NUMRUC || V_SSEPARADOR_CSV || V_EJE_APEPAT || V_SSEPARADOR_CSV || V_EJE_APEMAT ||
                   V_SSEPARADOR_CSV || V_EJE_NOMBRES || V_SSEPARADOR_CSV || V_EJE_OCUPACION || V_SSEPARADOR_CSV || V_EJE_PAIS || V_SSEPARADOR_CSV || V_EJE_CARGO || V_SSEPARADOR_CSV || V_EJE_PEP ||
                   V_SSEPARADOR_CSV || V_EJE_DOMICILIO || V_SSEPARADOR_CSV || V_EJE_DEPART || V_SSEPARADOR_CSV || V_EJE_PROV || V_SSEPARADOR_CSV || V_EJE_DIST || V_SSEPARADOR_CSV || V_EJE_TELEFONO ||
                   V_SSEPARADOR_CSV || V_ORD_RELACION || V_SSEPARADOR_CSV || V_ORD_CONDICION || V_SSEPARADOR_CSV || V_ORD_TIPPER || V_SSEPARADOR_CSV || V_ORD_TIPDOC || V_SSEPARADOR_CSV ||
                   V_ORD_NUMDOC || V_SSEPARADOR_CSV || V_ORD_NUMRUC || V_SSEPARADOR_CSV || V_ORD_APEPAT || V_SSEPARADOR_CSV || V_ORD_APEMAT || V_SSEPARADOR_CSV || V_ORD_NOMBRES || V_SSEPARADOR_CSV ||
                   V_ORD_OCUPACION || V_SSEPARADOR_CSV || V_ORD_PAIS || V_SSEPARADOR_CSV || V_ORD_CARGO || V_SSEPARADOR_CSV || V_ORD_PEP || V_SSEPARADOR_CSV || V_ORD_DIRECCION || V_SSEPARADOR_CSV ||
                   V_ORD_DOMICILIO || V_SSEPARADOR_CSV || V_UBIGEO || V_SSEPARADOR_CSV || V_ORD_DEPART || V_SSEPARADOR_CSV || V_ORD_PROV || V_SSEPARADOR_CSV || V_ORD_DIST || V_SSEPARADOR_CSV ||
                   V_ORD_TELEFONO || V_SSEPARADOR_CSV || V_BEN_RELACION || V_SSEPARADOR_CSV || V_BEN_CONDICION || V_SSEPARADOR_CSV || V_BEN_TIP_PER || V_SSEPARADOR_CSV || V_BEN_TIP_DOC ||
                   V_SSEPARADOR_CSV || V_BEN_NUM_DOC || V_SSEPARADOR_CSV || V_BEN_NUM_RUC || V_SSEPARADOR_CSV || V_BEN_APEPAT || V_SSEPARADOR_CSV || V_BEN_APEMAT || V_SSEPARADOR_CSV || V_BEN_NOMBRES ||
                   V_SSEPARADOR_CSV || V_BEN_OCUPACION || V_SSEPARADOR_CSV || V_BEN_PAIS || V_SSEPARADOR_CSV || V_BEN_CARGO || V_SSEPARADOR_CSV || V_BEN_PEP || V_SSEPARADOR_CSV || V_SSEPARADOR_CSV ||
                   V_BEN_DOMICILIO || V_SSEPARADOR_CSV || V_SSEPARADOR_CSV || V_BEN_DEPART || V_SSEPARADOR_CSV || V_BEN_PROV || V_SSEPARADOR_CSV || V_BEN_DIST || V_SSEPARADOR_CSV || V_BEN_TELEFONO ||
                   V_SSEPARADOR_CSV || V_DAT_TIPFON || V_SSEPARADOR_CSV || V_DAT_TIPOPE || V_SSEPARADOR_CSV || V_DAT_DESOPE || V_SSEPARADOR_CSV || V_DAT_ORIFON || V_SSEPARADOR_CSV || V_DAT_MONOPE ||
                   V_SSEPARADOR_CSV || V_DAT_MONOPE_A || V_SSEPARADOR_CSV || V_DAT_MTOOPE || V_SSEPARADOR_CSV || V_DAT_MTOOPEA || V_SSEPARADOR_CSV || V_DAT_COD_ENT_INVO || V_SSEPARADOR_CSV ||
                   V_DAT_COD_TIP_CTAO || V_SSEPARADOR_CSV || V_DAT_COD_CTAO || V_SSEPARADOR_CSV || V_DAT_ENT_FNC_EXTO || V_SSEPARADOR_CSV || V_DAT_COD_ENT_INVB || V_SSEPARADOR_CSV ||
                   V_DAT_COD_TIP_CTAB || V_SSEPARADOR_CSV || V_DAT_COD_CTAB || V_SSEPARADOR_CSV || V_DAT_ENT_FNC_EXTB || V_SSEPARADOR_CSV || V_DAT_ALCANCE || V_SSEPARADOR_CSV || V_DAT_COD_PAISO ||
                   V_SSEPARADOR_CSV || V_DAT_COD_PAISD || V_SSEPARADOR_CSV || V_DAT_INTOPE || V_SSEPARADOR_CSV || V_DAT_FORMA || V_SSEPARADOR_CSV || V_DAT_INFORM || V_SSEPARADOR_CSV || V_ORIGEN;

          UTL_FILE.PUT_LINE(FILE_OUTPUT, WTEXT);

        END LOOP;

      ELSIF P_EST_REPORTES = 'R' THEN

        RENTAS_MAYORES(P_FECINI, P_FECFIN, P_MONTO, P_TC, V_C_TABLE);

        WTEXT := '';
        SETEAR_VARIABLES;

        LOOP
          FETCH V_C_TABLE
            INTO V_FILA,
                 V_OPERACION,
                 V_OFICINA,
                 V_INTERNO,
                 V_MODALIDAD,
                 V_OPE_UBIGEO,
                 V_OPE_FECHA,
                 V_OPE_HORA,
                 V_EJE_RELACION,
                 V_EJE_CONDICION,
                 V_EJE_TIPPER,
                 V_EJE_TIPDOC,
                 V_EJE_NUMDOC,
                 V_EJE_NUMRUC,
                 V_EJE_APEPAT,
                 V_EJE_APEMAT,
                 V_EJE_NOMBRES,
                 V_EJE_OCUPACION,
                 V_EJE_PAIS,
                 V_EJE_CARGO,
                 V_EJE_PEP,
                 V_EJE_DOMICILIO,
                 V_EJE_DEPART,
                 V_EJE_PROV,
                 V_EJE_DIST,
                 V_EJE_TELEFONO,
                 V_ORD_RELACION,
                 V_ORD_CONDICION,
                 V_ORD_TIPPER,
                 V_ORD_TIPDOC,
                 V_ORD_NUMDOC,
                 V_ORD_NUMRUC,
                 V_ORD_APEPAT,
                 V_ORD_APEMAT,
                 V_ORD_NOMBRES,
                 V_ORD_OCUPACION,
                 V_ORD_PAIS,
                 V_ORD_CARGO,
                 V_ORD_PEP,
                 V_ORD_DOMICILIO,
                 V_ORD_DEPART,
                 V_ORD_PROV,
                 V_ORD_DIST,
                 V_ORD_TELEFONO,
                 V_BEN_RELACION,
                 V_BEN_CONDICION,
                 V_BEN_TIPPER,
                 V_BEN_TIPDOC,
                 V_BEN_NUMDOC,
                 V_BEN_NUMRUC,
                 V_BEN_APEPAT,
                 V_BEN_APEMAT,
                 V_BEN_NOMBRES,
                 V_BEN_OCUPACION,
                 V_BEN_PAIS,
                 V_BEN_CARGO,
                 V_BEN_PEP,
                 V_BEN_DOMICILIO,
                 V_BEN_DEPART,
                 V_BEN_PROV,
                 V_BEN_DIST,
                 V_BEN_TELEFONO,
                 V_DAT_TIPFON,
                 V_DAT_TIPOPE,
                 V_DAT_DESOPE,
                 V_DAT_ORIFON,
                 V_DAT_MONOPE,
                 V_DAT_MONOPEA,
                 V_DAT_MTOOPE,
                 V_DAT_MTOOPEA,
                 V_DAT_COD_ENT_INVO,
                 V_DAT_COD_TIP_CTAO,
                 V_DAT_COD_CTAO,
                 V_DAT_ENT_FNC_EXTO,
                 V_DAT_COD_ENT_INVB,
                 V_DAT_COD_TIP_CTAB,
                 V_DAT_COD_CTAB,
                 V_DAT_ENT_FNC_EXTB,
                 V_DAT_ALCANCE,
                 V_DAT_CODPAISO,
                 V_DAT_CODPAISD,
                 V_DAT_INTERMOPE,
                 V_DAT_FORMA,
                 V_DAT_INFORM,
                 --V_TIPO,
                 V_ORIGEN;

          EXIT WHEN V_C_TABLE%NOTFOUND;

          WTEXT := V_FILA || V_SSEPARADOR_CSV || V_OPERACION || V_SSEPARADOR_CSV || V_OFICINA || V_SSEPARADOR_CSV || V_INTERNO || V_SSEPARADOR_CSV || V_MODALIDAD || V_SSEPARADOR_CSV || V_OPE_UBIGEO ||
                   V_SSEPARADOR_CSV || V_OPE_FECHA || V_SSEPARADOR_CSV || V_OPE_HORA || V_SSEPARADOR_CSV || V_EJE_RELACION || V_SSEPARADOR_CSV || V_EJE_CONDICION || V_SSEPARADOR_CSV || V_EJE_TIPPER ||
                   V_SSEPARADOR_CSV || V_EJE_TIPDOC || V_SSEPARADOR_CSV || V_EJE_NUMDOC || V_SSEPARADOR_CSV || V_EJE_NUMRUC || V_SSEPARADOR_CSV || V_EJE_APEPAT || V_SSEPARADOR_CSV || V_EJE_APEMAT ||
                   V_SSEPARADOR_CSV || V_EJE_NOMBRES || V_SSEPARADOR_CSV || V_EJE_OCUPACION || V_SSEPARADOR_CSV || V_EJE_PAIS || V_SSEPARADOR_CSV || V_EJE_CARGO || V_SSEPARADOR_CSV || V_EJE_PEP ||
                   V_SSEPARADOR_CSV || V_EJE_DOMICILIO || V_SSEPARADOR_CSV || V_EJE_DEPART || V_SSEPARADOR_CSV || V_EJE_PROV || V_SSEPARADOR_CSV || V_EJE_DIST || V_SSEPARADOR_CSV || V_EJE_TELEFONO ||
                   V_SSEPARADOR_CSV || V_ORD_RELACION || V_SSEPARADOR_CSV || V_ORD_CONDICION || V_SSEPARADOR_CSV || V_ORD_TIPPER || V_SSEPARADOR_CSV || V_ORD_TIPDOC || V_SSEPARADOR_CSV ||
                   V_ORD_NUMDOC || V_SSEPARADOR_CSV || V_ORD_NUMRUC || V_SSEPARADOR_CSV || V_ORD_APEPAT || V_SSEPARADOR_CSV || V_ORD_APEMAT || V_SSEPARADOR_CSV || V_ORD_NOMBRES || V_SSEPARADOR_CSV ||
                   V_ORD_OCUPACION || V_SSEPARADOR_CSV || V_ORD_PAIS || V_SSEPARADOR_CSV || V_ORD_CARGO || V_SSEPARADOR_CSV || V_ORD_PEP || V_SSEPARADOR_CSV || V_ORD_DOMICILIO || V_SSEPARADOR_CSV ||
                   V_ORD_DEPART || V_SSEPARADOR_CSV || V_ORD_PROV || V_SSEPARADOR_CSV || V_ORD_DIST || V_SSEPARADOR_CSV || V_ORD_TELEFONO || V_SSEPARADOR_CSV || V_BEN_RELACION || V_SSEPARADOR_CSV ||
                   V_BEN_CONDICION || V_SSEPARADOR_CSV || V_BEN_TIPPER || V_SSEPARADOR_CSV || V_BEN_TIPDOC || V_SSEPARADOR_CSV || V_BEN_NUMDOC || V_SSEPARADOR_CSV || V_BEN_NUMRUC || V_SSEPARADOR_CSV ||
                   V_BEN_APEPAT || V_SSEPARADOR_CSV || V_BEN_APEMAT || V_SSEPARADOR_CSV || V_BEN_NOMBRES || V_SSEPARADOR_CSV || V_BEN_OCUPACION || V_SSEPARADOR_CSV || V_BEN_PAIS || V_SSEPARADOR_CSV ||
                   V_BEN_CARGO || V_SSEPARADOR_CSV || V_BEN_PEP || V_SSEPARADOR_CSV || V_BEN_DOMICILIO || V_SSEPARADOR_CSV || V_BEN_DEPART || V_SSEPARADOR_CSV || V_BEN_PROV || V_SSEPARADOR_CSV ||
                   V_BEN_DIST || V_SSEPARADOR_CSV || V_BEN_TELEFONO || V_SSEPARADOR_CSV || V_DAT_TIPFON || V_SSEPARADOR_CSV || V_DAT_TIPOPE || V_SSEPARADOR_CSV || V_DAT_DESOPE || V_SSEPARADOR_CSV ||
                   V_DAT_ORIFON || V_SSEPARADOR_CSV || V_DAT_MONOPE || V_SSEPARADOR_CSV || V_DAT_MONOPEA || V_SSEPARADOR_CSV || V_DAT_MTOOPE || V_SSEPARADOR_CSV || V_DAT_MTOOPEA || V_SSEPARADOR_CSV ||
                   V_DAT_COD_ENT_INVO || V_SSEPARADOR_CSV || V_DAT_COD_TIP_CTAO || V_SSEPARADOR_CSV || V_DAT_COD_CTAO || V_SSEPARADOR_CSV || V_DAT_ENT_FNC_EXTO || V_SSEPARADOR_CSV ||
                   V_DAT_COD_ENT_INVB || V_SSEPARADOR_CSV || V_DAT_COD_TIP_CTAB || V_SSEPARADOR_CSV || V_DAT_COD_CTAB || V_SSEPARADOR_CSV || V_DAT_ENT_FNC_EXTB || V_SSEPARADOR_CSV || V_DAT_ALCANCE ||
                   V_SSEPARADOR_CSV || V_DAT_CODPAISO || V_SSEPARADOR_CSV || V_DAT_CODPAISD || V_SSEPARADOR_CSV || V_DAT_INTERMOPE || V_SSEPARADOR_CSV || V_DAT_FORMA || V_SSEPARADOR_CSV ||
                   V_DAT_INFORM \*|| V_SSEPARADOR_CSV || V_TIPO*\
                   || V_SSEPARADOR_CSV || V_ORIGEN;

          UTL_FILE.PUT_LINE(FILE_OUTPUT, WTEXT);
        END LOOP;

      ELSIF P_EST_REPORTES = 'SV' THEN

        SINIESTROS_MAYORES(P_OPE, P_TC, P_MONTO, P_FECINI, P_FECFIN, V_C_TABLE);

        WTEXT := '';
        SETEAR_VARIABLES;

        LOOP
          FETCH V_C_TABLE
            INTO V_FILA,
                 V_OFICINA,
                 V_OPERACION,
                 V_INTERNO,
                 V_MODALIDAD,
                 V_OPE_UBIGEO,
                 V_OPE_FECHA,
                 V_OPE_HORA,
                 V_EJE_RELACION,
                 V_EJE_CONDICION,
                 V_EJE_TIPPER,
                 V_EJE_TIPDOC,
                 V_EJE_NUMDOC,
                 V_EJE_NUMRUC,
                 V_EJE_APEPAT,
                 V_EJE_APEMAT,
                 V_EJE_NOMBRES,
                 V_EJE_OCUPACION,
                 V_EJE_PAIS,
                 V_EJE_CARGO,
                 V_EJE_PEP,
                 V_EJE_DOMICILIO,
                 V_EJE_DEPART,
                 V_EJE_PROV,
                 V_EJE_DIST,
                 V_EJE_TELEFONO,
                 V_ORD_RELACION,
                 V_ORD_CONDICION,
                 V_ORD_TIPPER,
                 V_ORD_TIPDOC,
                 V_ORD_NUMDOC,
                 V_ORD_NUMRUC,
                 V_ORD_APEPAT,
                 V_ORD_APEMAT,
                 V_ORD_NOMBRES,
                 V_ORD_OCUPACION,
                 V_ORD_PAIS,
                 V_ORD_CARGO,
                 V_ORD_PEP,
                 V_ORD_DOMICILIO,
                 V_ORD_DEPART,
                 V_ORD_PROV,
                 V_ORD_DIST,
                 --
                 V_ORD_TELEFONO,
                 V_BEN_RELACION,
                 V_BEN_CONDICION,
                 V_BEN_TIP_PER,
                 V_BEN_TIP_DOC,
                 V_BEN_NUM_DOC,
                 V_BEN_NUM_RUC,
                 V_BEN_APEPAT,
                 V_BEN_APEMAT,
                 V_BEN_NOMBRES,
                 V_BEN_OCUPACION,
                 V_BEN_PAIS,
                 V_BEN_CARGO,
                 V_BEN_PEP,
                 V_BEN_DOMICILIO,
                 V_BEN_DEPART,
                 V_BEN_PROV,
                 V_BEN_DIST,
                 V_BEN_TELEFONO,
                 V_DAT_TIPFON,
                 V_DAT_TIPOPE,
                 V_DAT_DESOPE,
                 V_DAT_ORIFON,
                 V_DAT_MONOPE,
                 V_DAT_MONOPE_A,
                 V_DAT_MTOOPE,
                 V_DAT_MTOOPEA,
                 V_DAT_COD_ENT_INVO,
                 V_DAT_COD_TIP_CTAO,
                 V_DAT_COD_CTAO,
                 V_DAT_ENT_FNC_EXTO,
                 V_DAT_COD_ENT_INVB,
                 V_DAT_COD_TIP_CTAB,
                 V_DAT_COD_CTAB,
                 V_DAT_ENT_FNC_EXTB,
                 V_DAT_ALCANCE,
                 V_DAT_COD_PAISO,
                 V_DAT_COD_PAISD,
                 V_DAT_INTOPE,
                 V_DAT_FORMA,
                 V_DAT_INFORM,
                 V_ORIGEN;

          EXIT WHEN V_C_TABLE%NOTFOUND;

          --CUERPO DEL ARCHIVO
          WTEXT := V_FILA || V_SSEPARADOR_CSV || V_OFICINA || V_SSEPARADOR_CSV || V_OPERACION || V_SSEPARADOR_CSV || V_INTERNO || V_SSEPARADOR_CSV || V_MODALIDAD || V_SSEPARADOR_CSV || V_OPE_UBIGEO ||
                   V_SSEPARADOR_CSV || V_OPE_FECHA || V_SSEPARADOR_CSV || V_OPE_HORA || V_SSEPARADOR_CSV || V_EJE_RELACION || V_SSEPARADOR_CSV || V_EJE_CONDICION || V_SSEPARADOR_CSV || V_EJE_TIPPER ||
                   V_SSEPARADOR_CSV || V_EJE_TIPDOC || V_SSEPARADOR_CSV || V_EJE_NUMDOC || V_SSEPARADOR_CSV || V_EJE_NUMRUC || V_SSEPARADOR_CSV || V_EJE_APEPAT || V_SSEPARADOR_CSV || V_EJE_APEMAT ||
                   V_SSEPARADOR_CSV || V_EJE_NOMBRES || V_SSEPARADOR_CSV || V_EJE_OCUPACION || V_SSEPARADOR_CSV || V_EJE_PAIS || V_SSEPARADOR_CSV || V_EJE_CARGO || V_SSEPARADOR_CSV || V_EJE_PEP ||
                   V_SSEPARADOR_CSV || V_EJE_DOMICILIO || V_SSEPARADOR_CSV || V_EJE_DEPART || V_SSEPARADOR_CSV || V_EJE_PROV || V_SSEPARADOR_CSV || V_EJE_DIST || V_SSEPARADOR_CSV || V_EJE_TELEFONO ||
                   V_SSEPARADOR_CSV || V_ORD_RELACION || V_SSEPARADOR_CSV || V_ORD_CONDICION || V_SSEPARADOR_CSV || V_ORD_TIPPER || V_SSEPARADOR_CSV || V_ORD_TIPDOC || V_SSEPARADOR_CSV ||
                   V_ORD_NUMDOC || V_SSEPARADOR_CSV || V_ORD_NUMRUC || V_SSEPARADOR_CSV || V_ORD_APEPAT || V_SSEPARADOR_CSV || V_ORD_APEMAT || V_SSEPARADOR_CSV || V_ORD_NOMBRES || V_SSEPARADOR_CSV ||
                   V_ORD_OCUPACION || V_SSEPARADOR_CSV || V_ORD_PAIS || V_SSEPARADOR_CSV || V_ORD_CARGO || V_SSEPARADOR_CSV || V_ORD_PEP || V_SSEPARADOR_CSV || V_ORD_DOMICILIO || V_SSEPARADOR_CSV ||
                   V_ORD_DEPART || V_SSEPARADOR_CSV || V_ORD_PROV || V_SSEPARADOR_CSV || V_ORD_DIST || V_SSEPARADOR_CSV || V_ORD_TELEFONO || V_SSEPARADOR_CSV || V_BEN_RELACION || V_SSEPARADOR_CSV ||
                   V_BEN_CONDICION || V_SSEPARADOR_CSV || V_BEN_TIP_PER || V_SSEPARADOR_CSV || V_BEN_TIP_DOC || V_SSEPARADOR_CSV || V_BEN_NUM_DOC || V_SSEPARADOR_CSV || V_BEN_NUM_RUC ||
                   V_SSEPARADOR_CSV || V_BEN_APEPAT || V_SSEPARADOR_CSV || V_BEN_APEMAT || V_SSEPARADOR_CSV || V_BEN_NOMBRES || V_SSEPARADOR_CSV || V_BEN_OCUPACION || V_SSEPARADOR_CSV || V_BEN_PAIS ||
                   V_SSEPARADOR_CSV || V_BEN_CARGO || V_SSEPARADOR_CSV || V_BEN_PEP || V_SSEPARADOR_CSV || V_BEN_DOMICILIO || V_SSEPARADOR_CSV || V_SSEPARADOR_CSV || V_BEN_DEPART || V_SSEPARADOR_CSV ||
                   V_BEN_PROV || V_SSEPARADOR_CSV || V_BEN_DIST || V_SSEPARADOR_CSV || V_BEN_TELEFONO || V_SSEPARADOR_CSV || V_DAT_TIPFON || V_SSEPARADOR_CSV || V_DAT_TIPOPE || V_SSEPARADOR_CSV ||
                   V_DAT_DESOPE || V_SSEPARADOR_CSV || V_DAT_ORIFON || V_SSEPARADOR_CSV || V_DAT_MONOPE || V_SSEPARADOR_CSV || V_DAT_MONOPE_A || V_SSEPARADOR_CSV || V_DAT_MTOOPE || V_SSEPARADOR_CSV ||
                   V_DAT_MTOOPEA || V_SSEPARADOR_CSV || V_DAT_COD_ENT_INVO || V_SSEPARADOR_CSV || V_DAT_COD_TIP_CTAO || V_SSEPARADOR_CSV || V_DAT_COD_CTAO || V_SSEPARADOR_CSV || V_DAT_ENT_FNC_EXTO ||
                   V_SSEPARADOR_CSV || V_DAT_COD_ENT_INVB || V_SSEPARADOR_CSV || V_DAT_COD_TIP_CTAB || V_SSEPARADOR_CSV || V_DAT_COD_CTAB || V_SSEPARADOR_CSV || V_DAT_ENT_FNC_EXTB || V_SSEPARADOR_CSV ||
                   V_DAT_ALCANCE || V_SSEPARADOR_CSV || V_DAT_COD_PAISO || V_SSEPARADOR_CSV || V_DAT_COD_PAISD || V_SSEPARADOR_CSV || V_DAT_INTOPE || V_SSEPARADOR_CSV || V_DAT_FORMA ||
                   V_SSEPARADOR_CSV || V_DAT_INFORM || V_SSEPARADOR_CSV || V_ORIGEN;

          UTL_FILE.PUT_LINE(FILE_OUTPUT, WTEXT);
        END LOOP;

        VIDA_MAYORES(P_OPE, P_TC, P_MONTO, P_FECINI, P_FECFIN, V_C_TABLE);

        WTEXT := '';
        SETEAR_VARIABLES;

        LOOP
          FETCH V_C_TABLE
            INTO V_FILA,
                 V_OFICINA,
                 V_OPERACION,
                 V_INTERNO,
                 V_MODALIDAD,
                 V_OPE_UBIGEO,
                 V_OPE_FECHA,
                 V_OPE_HORA,
                 V_EJE_RELACION,
                 V_EJE_CONDICION,
                 V_EJE_TIPPER,
                 V_EJE_TIPDOC,
                 V_EJE_NUMDOC,
                 V_EJE_NUMRUC,
                 V_EJE_APEPAT,
                 V_EJE_APEMAT,
                 V_EJE_NOMBRES,
                 V_EJE_OCUPACION,
                 V_EJE_PAIS,
                 V_EJE_CARGO,
                 V_EJE_PEP,
                 V_EJE_DOMICILIO,
                 V_EJE_DEPART,
                 V_EJE_PROV,
                 V_EJE_DIST,
                 V_EJE_TELEFONO,
                 V_ORD_RELACION,
                 V_ORD_CONDICION,
                 V_ORD_TIPPER,
                 V_ORD_TIPDOC,
                 V_ORD_NUMDOC,
                 V_ORD_NUMRUC,
                 V_ORD_APEPAT,
                 V_ORD_APEMAT,
                 V_ORD_NOMBRES,
                 V_ORD_OCUPACION,
                 V_ORD_PAIS,
                 V_ORD_CARGO,
                 V_ORD_PEP,
                 V_ORD_DOMICILIO,
                 V_ORD_DEPART,
                 V_ORD_PROV,
                 V_ORD_DIST,
                 V_ORD_TELEFONO,
                 V_BEN_RELACION,
                 V_BEN_CONDICION,
                 V_BEN_TIP_PER,
                 V_BEN_TIP_DOC,
                 V_BEN_NUM_DOC,
                 V_BEN_NUM_RUC,
                 V_BEN_APEPAT,
                 V_BEN_APEMAT,
                 V_BEN_NOMBRES,
                 V_BEN_OCUPACION,
                 V_BEN_PAIS,
                 V_BEN_CARGO,
                 V_BEN_PEP,
                 V_BEN_DOMICILIO,
                 V_BEN_DEPART,
                 V_BEN_PROV,
                 V_BEN_DIST,
                 V_BEN_TELEFONO,
                 V_DAT_TIPFON,
                 V_DAT_TIPOPE,
                 V_DAT_DESOPE,
                 V_DAT_ORIFON,
                 V_DAT_MONOPE,
                 V_DAT_MONOPE_A,
                 V_DAT_MTOOPE,
                 V_DAT_MTOOPEA,
                 V_DAT_COD_ENT_INVO,
                 V_DAT_COD_TIP_CTAO,
                 V_DAT_COD_CTAO,
                 V_DAT_ENT_FNC_EXTO,
                 V_DAT_COD_ENT_INVB,
                 V_DAT_COD_TIP_CTAB,
                 V_DAT_COD_CTAB,
                 V_DAT_ENT_FNC_EXTB,
                 V_DAT_ALCANCE,
                 V_DAT_COD_PAISO,
                 V_DAT_COD_PAISD,
                 V_DAT_INTOPE,
                 V_DAT_FORMA,
                 V_DAT_INFORM,
                 V_ORIGEN;

          EXIT WHEN V_C_TABLE%NOTFOUND;

          --CUERPO DEL ARCHIVO
          WTEXT := V_FILA || V_SSEPARADOR_CSV || V_OFICINA || V_SSEPARADOR_CSV || V_OPERACION || V_SSEPARADOR_CSV || V_INTERNO || V_SSEPARADOR_CSV || V_MODALIDAD || V_SSEPARADOR_CSV || V_OPE_UBIGEO ||
                   V_SSEPARADOR_CSV || V_OPE_FECHA || V_SSEPARADOR_CSV || V_OPE_HORA || V_SSEPARADOR_CSV || V_EJE_RELACION || V_SSEPARADOR_CSV || V_EJE_CONDICION || V_SSEPARADOR_CSV || V_EJE_TIPPER ||
                   V_SSEPARADOR_CSV || V_EJE_TIPDOC || V_SSEPARADOR_CSV || V_EJE_NUMDOC || V_SSEPARADOR_CSV || V_EJE_NUMRUC || V_SSEPARADOR_CSV || V_EJE_APEPAT || V_SSEPARADOR_CSV || V_EJE_APEMAT ||
                   V_SSEPARADOR_CSV || V_EJE_NOMBRES || V_SSEPARADOR_CSV || V_EJE_OCUPACION || V_SSEPARADOR_CSV || V_EJE_PAIS || V_SSEPARADOR_CSV || V_EJE_CARGO || V_SSEPARADOR_CSV || V_EJE_PEP ||
                   V_SSEPARADOR_CSV || V_EJE_DOMICILIO || V_SSEPARADOR_CSV || V_EJE_DEPART || V_SSEPARADOR_CSV || V_EJE_PROV || V_SSEPARADOR_CSV || V_EJE_DIST || V_SSEPARADOR_CSV || V_EJE_TELEFONO ||
                   V_SSEPARADOR_CSV || V_ORD_RELACION || V_SSEPARADOR_CSV || V_ORD_CONDICION || V_SSEPARADOR_CSV || V_ORD_TIPPER || V_SSEPARADOR_CSV || V_ORD_TIPDOC || V_SSEPARADOR_CSV ||
                   V_ORD_NUMDOC || V_SSEPARADOR_CSV || V_ORD_NUMRUC || V_SSEPARADOR_CSV || V_ORD_APEPAT || V_SSEPARADOR_CSV || V_ORD_APEMAT || V_SSEPARADOR_CSV || V_ORD_NOMBRES || V_SSEPARADOR_CSV ||
                   V_ORD_OCUPACION || V_SSEPARADOR_CSV || V_ORD_PAIS || V_SSEPARADOR_CSV || V_ORD_CARGO || V_SSEPARADOR_CSV || V_ORD_PEP || V_SSEPARADOR_CSV || V_ORD_DIRECCION || V_SSEPARADOR_CSV ||
                   V_ORD_DOMICILIO || V_SSEPARADOR_CSV || V_UBIGEO || V_SSEPARADOR_CSV || V_ORD_DEPART || V_SSEPARADOR_CSV || V_ORD_PROV || V_SSEPARADOR_CSV || V_ORD_DIST || V_SSEPARADOR_CSV ||
                   V_ORD_TELEFONO || V_SSEPARADOR_CSV || V_BEN_RELACION || V_SSEPARADOR_CSV || V_BEN_CONDICION || V_SSEPARADOR_CSV || V_BEN_TIP_PER || V_SSEPARADOR_CSV || V_BEN_TIP_DOC ||
                   V_SSEPARADOR_CSV || V_BEN_NUM_DOC || V_SSEPARADOR_CSV || V_BEN_NUM_RUC || V_SSEPARADOR_CSV || V_BEN_APEPAT || V_SSEPARADOR_CSV || V_BEN_APEMAT || V_SSEPARADOR_CSV || V_BEN_NOMBRES ||
                   V_SSEPARADOR_CSV || V_BEN_OCUPACION || V_SSEPARADOR_CSV || V_BEN_PAIS || V_SSEPARADOR_CSV || V_BEN_CARGO || V_SSEPARADOR_CSV || V_BEN_PEP || V_SSEPARADOR_CSV || V_SSEPARADOR_CSV ||
                   V_BEN_DOMICILIO || V_SSEPARADOR_CSV || V_SSEPARADOR_CSV || V_BEN_DEPART || V_SSEPARADOR_CSV || V_BEN_PROV || V_SSEPARADOR_CSV || V_BEN_DIST || V_SSEPARADOR_CSV || V_BEN_TELEFONO ||
                   V_SSEPARADOR_CSV || V_DAT_TIPFON || V_SSEPARADOR_CSV || V_DAT_TIPOPE || V_SSEPARADOR_CSV || V_DAT_DESOPE || V_SSEPARADOR_CSV || V_DAT_ORIFON || V_SSEPARADOR_CSV || V_DAT_MONOPE ||
                   V_SSEPARADOR_CSV || V_DAT_MONOPE_A || V_SSEPARADOR_CSV || V_DAT_MTOOPE || V_SSEPARADOR_CSV || V_DAT_MTOOPEA || V_SSEPARADOR_CSV || V_DAT_COD_ENT_INVO || V_SSEPARADOR_CSV ||
                   V_DAT_COD_TIP_CTAO || V_SSEPARADOR_CSV || V_DAT_COD_CTAO || V_SSEPARADOR_CSV || V_DAT_ENT_FNC_EXTO || V_SSEPARADOR_CSV || V_DAT_COD_ENT_INVB || V_SSEPARADOR_CSV ||
                   V_DAT_COD_TIP_CTAB || V_SSEPARADOR_CSV || V_DAT_COD_CTAB || V_SSEPARADOR_CSV || V_DAT_ENT_FNC_EXTB || V_SSEPARADOR_CSV || V_DAT_ALCANCE || V_SSEPARADOR_CSV || V_DAT_COD_PAISO ||
                   V_SSEPARADOR_CSV || V_DAT_COD_PAISD || V_SSEPARADOR_CSV || V_DAT_INTOPE || V_SSEPARADOR_CSV || V_DAT_FORMA || V_SSEPARADOR_CSV || V_DAT_INFORM || V_SSEPARADOR_CSV || V_ORIGEN;

          UTL_FILE.PUT_LINE(FILE_OUTPUT, WTEXT);

        END LOOP;

      ELSIF P_EST_REPORTES = 'SR' THEN

        SINIESTROS_MAYORES(P_OPE, P_TC, P_MONTO, P_FECINI, P_FECFIN, V_C_TABLE);

        WTEXT := '';
        SETEAR_VARIABLES;

        LOOP
          FETCH V_C_TABLE
            INTO V_FILA,
                 V_OFICINA,
                 V_OPERACION,
                 V_INTERNO,
                 V_MODALIDAD,
                 V_OPE_UBIGEO,
                 V_OPE_FECHA,
                 V_OPE_HORA,
                 V_EJE_RELACION,
                 V_EJE_CONDICION,
                 V_EJE_TIPPER,
                 V_EJE_TIPDOC,
                 V_EJE_NUMDOC,
                 V_EJE_NUMRUC,
                 V_EJE_APEPAT,
                 V_EJE_APEMAT,
                 V_EJE_NOMBRES,
                 V_EJE_OCUPACION,
                 V_EJE_PAIS,
                 V_EJE_CARGO,
                 V_EJE_PEP,
                 V_EJE_DOMICILIO,
                 V_EJE_DEPART,
                 V_EJE_PROV,
                 V_EJE_DIST,
                 V_EJE_TELEFONO,
                 V_ORD_RELACION,
                 V_ORD_CONDICION,
                 V_ORD_TIPPER,
                 V_ORD_TIPDOC,
                 V_ORD_NUMDOC,
                 V_ORD_NUMRUC,
                 V_ORD_APEPAT,
                 V_ORD_APEMAT,
                 V_ORD_NOMBRES,
                 V_ORD_OCUPACION,
                 V_ORD_PAIS,
                 V_ORD_CARGO,
                 V_ORD_PEP,
                 V_ORD_DOMICILIO,
                 V_ORD_DEPART,
                 V_ORD_PROV,
                 V_ORD_DIST,
                 --
                 V_ORD_TELEFONO,
                 V_BEN_RELACION,
                 V_BEN_CONDICION,
                 V_BEN_TIP_PER,
                 V_BEN_TIP_DOC,
                 V_BEN_NUM_DOC,
                 V_BEN_NUM_RUC,
                 V_BEN_APEPAT,
                 V_BEN_APEMAT,
                 V_BEN_NOMBRES,
                 V_BEN_OCUPACION,
                 V_BEN_PAIS,
                 V_BEN_CARGO,
                 V_BEN_PEP,
                 V_BEN_DOMICILIO,
                 V_BEN_DEPART,
                 V_BEN_PROV,
                 V_BEN_DIST,
                 V_BEN_TELEFONO,
                 V_DAT_TIPFON,
                 V_DAT_TIPOPE,
                 V_DAT_DESOPE,
                 V_DAT_ORIFON,
                 V_DAT_MONOPE,
                 V_DAT_MONOPE_A,
                 V_DAT_MTOOPE,
                 V_DAT_MTOOPEA,
                 V_DAT_COD_ENT_INVO,
                 V_DAT_COD_TIP_CTAO,
                 V_DAT_COD_CTAO,
                 V_DAT_ENT_FNC_EXTO,
                 V_DAT_COD_ENT_INVB,
                 V_DAT_COD_TIP_CTAB,
                 V_DAT_COD_CTAB,
                 V_DAT_ENT_FNC_EXTB,
                 V_DAT_ALCANCE,
                 V_DAT_COD_PAISO,
                 V_DAT_COD_PAISD,
                 V_DAT_INTOPE,
                 V_DAT_FORMA,
                 V_DAT_INFORM,
                 V_ORIGEN;

          EXIT WHEN V_C_TABLE%NOTFOUND;

          --CUERPO DEL ARCHIVO
          WTEXT := V_FILA || V_SSEPARADOR_CSV || V_OFICINA || V_SSEPARADOR_CSV || V_OPERACION || V_SSEPARADOR_CSV || V_INTERNO || V_SSEPARADOR_CSV || V_MODALIDAD || V_SSEPARADOR_CSV || V_OPE_UBIGEO ||
                   V_SSEPARADOR_CSV || V_OPE_FECHA || V_SSEPARADOR_CSV || V_OPE_HORA || V_SSEPARADOR_CSV || V_EJE_RELACION || V_SSEPARADOR_CSV || V_EJE_CONDICION || V_SSEPARADOR_CSV || V_EJE_TIPPER ||
                   V_SSEPARADOR_CSV || V_EJE_TIPDOC || V_SSEPARADOR_CSV || V_EJE_NUMDOC || V_SSEPARADOR_CSV || V_EJE_NUMRUC || V_SSEPARADOR_CSV || V_EJE_APEPAT || V_SSEPARADOR_CSV || V_EJE_APEMAT ||
                   V_SSEPARADOR_CSV || V_EJE_NOMBRES || V_SSEPARADOR_CSV || V_EJE_OCUPACION || V_SSEPARADOR_CSV || V_EJE_PAIS || V_SSEPARADOR_CSV || V_EJE_CARGO || V_SSEPARADOR_CSV || V_EJE_PEP ||
                   V_SSEPARADOR_CSV || V_EJE_DOMICILIO || V_SSEPARADOR_CSV || V_EJE_DEPART || V_SSEPARADOR_CSV || V_EJE_PROV || V_SSEPARADOR_CSV || V_EJE_DIST || V_SSEPARADOR_CSV || V_EJE_TELEFONO ||
                   V_SSEPARADOR_CSV || V_ORD_RELACION || V_SSEPARADOR_CSV || V_ORD_CONDICION || V_SSEPARADOR_CSV || V_ORD_TIPPER || V_SSEPARADOR_CSV || V_ORD_TIPDOC || V_SSEPARADOR_CSV ||
                   V_ORD_NUMDOC || V_SSEPARADOR_CSV || V_ORD_NUMRUC || V_SSEPARADOR_CSV || V_ORD_APEPAT || V_SSEPARADOR_CSV || V_ORD_APEMAT || V_SSEPARADOR_CSV || V_ORD_NOMBRES || V_SSEPARADOR_CSV ||
                   V_ORD_OCUPACION || V_SSEPARADOR_CSV || V_ORD_PAIS || V_SSEPARADOR_CSV || V_ORD_CARGO || V_SSEPARADOR_CSV || V_ORD_PEP || V_SSEPARADOR_CSV || V_ORD_DOMICILIO || V_SSEPARADOR_CSV ||
                   V_ORD_DEPART || V_SSEPARADOR_CSV || V_ORD_PROV || V_SSEPARADOR_CSV || V_ORD_DIST || V_SSEPARADOR_CSV || V_ORD_TELEFONO || V_SSEPARADOR_CSV || V_BEN_RELACION || V_SSEPARADOR_CSV ||
                   V_BEN_CONDICION || V_SSEPARADOR_CSV || V_BEN_TIP_PER || V_SSEPARADOR_CSV || V_BEN_TIP_DOC || V_SSEPARADOR_CSV || V_BEN_NUM_DOC || V_SSEPARADOR_CSV || V_BEN_NUM_RUC ||
                   V_SSEPARADOR_CSV || V_BEN_APEPAT || V_SSEPARADOR_CSV || V_BEN_APEMAT || V_SSEPARADOR_CSV || V_BEN_NOMBRES || V_SSEPARADOR_CSV || V_BEN_OCUPACION || V_SSEPARADOR_CSV || V_BEN_PAIS ||
                   V_SSEPARADOR_CSV || V_BEN_CARGO || V_SSEPARADOR_CSV || V_BEN_PEP || V_SSEPARADOR_CSV || V_BEN_DOMICILIO || V_SSEPARADOR_CSV || V_SSEPARADOR_CSV || V_BEN_DEPART || V_SSEPARADOR_CSV ||
                   V_BEN_PROV || V_SSEPARADOR_CSV || V_BEN_DIST || V_SSEPARADOR_CSV || V_BEN_TELEFONO || V_SSEPARADOR_CSV || V_DAT_TIPFON || V_SSEPARADOR_CSV || V_DAT_TIPOPE || V_SSEPARADOR_CSV ||
                   V_DAT_DESOPE || V_SSEPARADOR_CSV || V_DAT_ORIFON || V_SSEPARADOR_CSV || V_DAT_MONOPE || V_SSEPARADOR_CSV || V_DAT_MONOPE_A || V_SSEPARADOR_CSV || V_DAT_MTOOPE || V_SSEPARADOR_CSV ||
                   V_DAT_MTOOPEA || V_SSEPARADOR_CSV || V_DAT_COD_ENT_INVO || V_SSEPARADOR_CSV || V_DAT_COD_TIP_CTAO || V_SSEPARADOR_CSV || V_DAT_COD_CTAO || V_SSEPARADOR_CSV || V_DAT_ENT_FNC_EXTO ||
                   V_SSEPARADOR_CSV || V_DAT_COD_ENT_INVB || V_SSEPARADOR_CSV || V_DAT_COD_TIP_CTAB || V_SSEPARADOR_CSV || V_DAT_COD_CTAB || V_SSEPARADOR_CSV || V_DAT_ENT_FNC_EXTB || V_SSEPARADOR_CSV ||
                   V_DAT_ALCANCE || V_SSEPARADOR_CSV || V_DAT_COD_PAISO || V_SSEPARADOR_CSV || V_DAT_COD_PAISD || V_SSEPARADOR_CSV || V_DAT_INTOPE || V_SSEPARADOR_CSV || V_DAT_FORMA ||
                   V_SSEPARADOR_CSV || V_DAT_INFORM || V_SSEPARADOR_CSV || V_ORIGEN;

          UTL_FILE.PUT_LINE(FILE_OUTPUT, WTEXT);
        END LOOP;

        RENTAS_MAYORES(P_FECINI, P_FECFIN, P_MONTO, P_TC, V_C_TABLE);

        WTEXT := '';
        SETEAR_VARIABLES;

        LOOP
          FETCH V_C_TABLE
            INTO V_FILA,
                 V_OPERACION,
                 V_OFICINA,
                 V_INTERNO,
                 V_MODALIDAD,
                 V_OPE_UBIGEO,
                 V_OPE_FECHA,
                 V_OPE_HORA,
                 V_EJE_RELACION,
                 V_EJE_CONDICION,
                 V_EJE_TIPPER,
                 V_EJE_TIPDOC,
                 V_EJE_NUMDOC,
                 V_EJE_NUMRUC,
                 V_EJE_APEPAT,
                 V_EJE_APEMAT,
                 V_EJE_NOMBRES,
                 V_EJE_OCUPACION,
                 V_EJE_PAIS,
                 V_EJE_CARGO,
                 V_EJE_PEP,
                 V_EJE_DOMICILIO,
                 V_EJE_DEPART,
                 V_EJE_PROV,
                 V_EJE_DIST,
                 V_EJE_TELEFONO,
                 V_ORD_RELACION,
                 V_ORD_CONDICION,
                 V_ORD_TIPPER,
                 V_ORD_TIPDOC,
                 V_ORD_NUMDOC,
                 V_ORD_NUMRUC,
                 V_ORD_APEPAT,
                 V_ORD_APEMAT,
                 V_ORD_NOMBRES,
                 V_ORD_OCUPACION,
                 V_ORD_PAIS,
                 V_ORD_CARGO,
                 V_ORD_PEP,
                 V_ORD_DOMICILIO,
                 V_ORD_DEPART,
                 V_ORD_PROV,
                 V_ORD_DIST,
                 V_ORD_TELEFONO,
                 V_BEN_RELACION,
                 V_BEN_CONDICION,
                 V_BEN_TIPPER,
                 V_BEN_TIPDOC,
                 V_BEN_NUMDOC,
                 V_BEN_NUMRUC,
                 V_BEN_APEPAT,
                 V_BEN_APEMAT,
                 V_BEN_NOMBRES,
                 V_BEN_OCUPACION,
                 V_BEN_PAIS,
                 V_BEN_CARGO,
                 V_BEN_PEP,
                 V_BEN_DOMICILIO,
                 V_BEN_DEPART,
                 V_BEN_PROV,
                 V_BEN_DIST,
                 V_BEN_TELEFONO,
                 V_DAT_TIPFON,
                 V_DAT_TIPOPE,
                 V_DAT_DESOPE,
                 V_DAT_ORIFON,
                 V_DAT_MONOPE,
                 V_DAT_MONOPEA,
                 V_DAT_MTOOPE,
                 V_DAT_MTOOPEA,
                 V_DAT_COD_ENT_INVO,
                 V_DAT_COD_TIP_CTAO,
                 V_DAT_COD_CTAO,
                 V_DAT_ENT_FNC_EXTO,
                 V_DAT_COD_ENT_INVB,
                 V_DAT_COD_TIP_CTAB,
                 V_DAT_COD_CTAB,
                 V_DAT_ENT_FNC_EXTB,
                 V_DAT_ALCANCE,
                 V_DAT_CODPAISO,
                 V_DAT_CODPAISD,
                 V_DAT_INTERMOPE,
                 V_DAT_FORMA,
                 V_DAT_INFORM,
                 V_TIPO,
                 V_ORIGEN;

          EXIT WHEN V_C_TABLE%NOTFOUND;

          WTEXT := V_FILA || V_SSEPARADOR_CSV || V_OPERACION || V_SSEPARADOR_CSV || V_OFICINA || V_SSEPARADOR_CSV || V_INTERNO || V_SSEPARADOR_CSV || V_MODALIDAD || V_SSEPARADOR_CSV || V_OPE_UBIGEO ||
                   V_SSEPARADOR_CSV || V_OPE_FECHA || V_SSEPARADOR_CSV || V_OPE_HORA || V_SSEPARADOR_CSV || V_EJE_RELACION || V_SSEPARADOR_CSV || V_EJE_CONDICION || V_SSEPARADOR_CSV || V_EJE_TIPPER ||
                   V_SSEPARADOR_CSV || V_EJE_TIPDOC || V_SSEPARADOR_CSV || V_EJE_NUMDOC || V_SSEPARADOR_CSV || V_EJE_NUMRUC || V_SSEPARADOR_CSV || V_EJE_APEPAT || V_SSEPARADOR_CSV || V_EJE_APEMAT ||
                   V_SSEPARADOR_CSV || V_EJE_NOMBRES || V_SSEPARADOR_CSV || V_EJE_OCUPACION || V_SSEPARADOR_CSV || V_EJE_PAIS || V_SSEPARADOR_CSV || V_EJE_CARGO || V_SSEPARADOR_CSV || V_EJE_PEP ||
                   V_SSEPARADOR_CSV || V_EJE_DOMICILIO || V_SSEPARADOR_CSV || V_EJE_DEPART || V_SSEPARADOR_CSV || V_EJE_PROV || V_SSEPARADOR_CSV || V_EJE_DIST || V_SSEPARADOR_CSV || V_EJE_TELEFONO ||
                   V_SSEPARADOR_CSV || V_ORD_RELACION || V_SSEPARADOR_CSV || V_ORD_CONDICION || V_SSEPARADOR_CSV || V_ORD_TIPPER || V_SSEPARADOR_CSV || V_ORD_TIPDOC || V_SSEPARADOR_CSV ||
                   V_ORD_NUMDOC || V_SSEPARADOR_CSV || V_ORD_NUMRUC || V_SSEPARADOR_CSV || V_ORD_APEPAT || V_SSEPARADOR_CSV || V_ORD_APEMAT || V_SSEPARADOR_CSV || V_ORD_NOMBRES || V_SSEPARADOR_CSV ||
                   V_ORD_OCUPACION || V_SSEPARADOR_CSV || V_ORD_PAIS || V_SSEPARADOR_CSV || V_ORD_CARGO || V_SSEPARADOR_CSV || V_ORD_PEP || V_SSEPARADOR_CSV || V_ORD_DOMICILIO || V_SSEPARADOR_CSV ||
                   V_ORD_DEPART || V_SSEPARADOR_CSV || V_ORD_PROV || V_SSEPARADOR_CSV || V_ORD_DIST || V_SSEPARADOR_CSV || V_ORD_TELEFONO || V_SSEPARADOR_CSV || V_BEN_RELACION || V_SSEPARADOR_CSV ||
                   V_BEN_CONDICION || V_SSEPARADOR_CSV || V_BEN_TIPPER || V_SSEPARADOR_CSV || V_BEN_TIPDOC || V_SSEPARADOR_CSV || V_BEN_NUMDOC || V_SSEPARADOR_CSV || V_BEN_NUMRUC || V_SSEPARADOR_CSV ||
                   V_BEN_APEPAT || V_SSEPARADOR_CSV || V_BEN_APEMAT || V_SSEPARADOR_CSV || V_BEN_NOMBRES || V_SSEPARADOR_CSV || V_BEN_OCUPACION || V_SSEPARADOR_CSV || V_BEN_PAIS || V_SSEPARADOR_CSV ||
                   V_BEN_CARGO || V_SSEPARADOR_CSV || V_BEN_PEP || V_SSEPARADOR_CSV || V_BEN_DOMICILIO || V_SSEPARADOR_CSV || V_BEN_DEPART || V_SSEPARADOR_CSV || V_BEN_PROV || V_SSEPARADOR_CSV ||
                   V_BEN_DIST || V_SSEPARADOR_CSV || V_BEN_TELEFONO || V_SSEPARADOR_CSV || V_DAT_TIPFON || V_SSEPARADOR_CSV || V_DAT_TIPOPE || V_SSEPARADOR_CSV || V_DAT_DESOPE || V_SSEPARADOR_CSV ||
                   V_DAT_ORIFON || V_SSEPARADOR_CSV || V_DAT_MONOPE || V_SSEPARADOR_CSV || V_DAT_MONOPEA || V_SSEPARADOR_CSV || V_DAT_MTOOPE || V_SSEPARADOR_CSV || V_DAT_MTOOPEA || V_SSEPARADOR_CSV ||
                   V_DAT_COD_ENT_INVO || V_SSEPARADOR_CSV || V_DAT_COD_TIP_CTAO || V_SSEPARADOR_CSV || V_DAT_COD_CTAO || V_SSEPARADOR_CSV || V_DAT_ENT_FNC_EXTO || V_SSEPARADOR_CSV ||
                   V_DAT_COD_ENT_INVB || V_SSEPARADOR_CSV || V_DAT_COD_TIP_CTAB || V_SSEPARADOR_CSV || V_DAT_COD_CTAB || V_SSEPARADOR_CSV || V_DAT_ENT_FNC_EXTB || V_SSEPARADOR_CSV || V_DAT_ALCANCE ||
                   V_SSEPARADOR_CSV || V_DAT_CODPAISO || V_SSEPARADOR_CSV || V_DAT_CODPAISD || V_SSEPARADOR_CSV || V_DAT_INTERMOPE || V_SSEPARADOR_CSV || V_DAT_FORMA || V_SSEPARADOR_CSV ||
                   V_DAT_INFORM || V_SSEPARADOR_CSV || V_TIPO || V_SSEPARADOR_CSV || V_ORIGEN;

          UTL_FILE.PUT_LINE(FILE_OUTPUT, WTEXT);
        END LOOP;

      ELSIF P_EST_REPORTES = 'VR' THEN

        VIDA_MAYORES(P_OPE, P_TC, P_MONTO, P_FECINI, P_FECFIN, V_C_TABLE);

        WTEXT := '';
        SETEAR_VARIABLES;

        LOOP
          FETCH V_C_TABLE
            INTO V_FILA,
                 V_OFICINA,
                 V_OPERACION,
                 V_INTERNO,
                 V_MODALIDAD,
                 V_OPE_UBIGEO,
                 V_OPE_FECHA,
                 V_OPE_HORA,
                 V_EJE_RELACION,
                 V_EJE_CONDICION,
                 V_EJE_TIPPER,
                 V_EJE_TIPDOC,
                 V_EJE_NUMDOC,
                 V_EJE_NUMRUC,
                 V_EJE_APEPAT,
                 V_EJE_APEMAT,
                 V_EJE_NOMBRES,
                 V_EJE_OCUPACION,
                 V_EJE_PAIS,
                 V_EJE_CARGO,
                 V_EJE_PEP,
                 V_EJE_DOMICILIO,
                 V_EJE_DEPART,
                 V_EJE_PROV,
                 V_EJE_DIST,
                 V_EJE_TELEFONO,
                 V_ORD_RELACION,
                 V_ORD_CONDICION,
                 V_ORD_TIPPER,
                 V_ORD_TIPDOC,
                 V_ORD_NUMDOC,
                 V_ORD_NUMRUC,
                 V_ORD_APEPAT,
                 V_ORD_APEMAT,
                 V_ORD_NOMBRES,
                 V_ORD_OCUPACION,
                 V_ORD_PAIS,
                 V_ORD_CARGO,
                 V_ORD_PEP,
                 V_ORD_DOMICILIO,
                 V_ORD_DEPART,
                 V_ORD_PROV,
                 V_ORD_DIST,
                 V_ORD_TELEFONO,
                 V_BEN_RELACION,
                 V_BEN_CONDICION,
                 V_BEN_TIP_PER,
                 V_BEN_TIP_DOC,
                 V_BEN_NUM_DOC,
                 V_BEN_NUM_RUC,
                 V_BEN_APEPAT,
                 V_BEN_APEMAT,
                 V_BEN_NOMBRES,
                 V_BEN_OCUPACION,
                 V_BEN_PAIS,
                 V_BEN_CARGO,
                 V_BEN_PEP,
                 V_BEN_DOMICILIO,
                 V_BEN_DEPART,
                 V_BEN_PROV,
                 V_BEN_DIST,
                 V_BEN_TELEFONO,
                 V_DAT_TIPFON,
                 V_DAT_TIPOPE,
                 V_DAT_DESOPE,
                 V_DAT_ORIFON,
                 V_DAT_MONOPE,
                 V_DAT_MONOPE_A,
                 V_DAT_MTOOPE,
                 V_DAT_MTOOPEA,
                 V_DAT_COD_ENT_INVO,
                 V_DAT_COD_TIP_CTAO,
                 V_DAT_COD_CTAO,
                 V_DAT_ENT_FNC_EXTO,
                 V_DAT_COD_ENT_INVB,
                 V_DAT_COD_TIP_CTAB,
                 V_DAT_COD_CTAB,
                 V_DAT_ENT_FNC_EXTB,
                 V_DAT_ALCANCE,
                 V_DAT_COD_PAISO,
                 V_DAT_COD_PAISD,
                 V_DAT_INTOPE,
                 V_DAT_FORMA,
                 V_DAT_INFORM,
                 V_ORIGEN;

          EXIT WHEN V_C_TABLE%NOTFOUND;

          --CUERPO DEL ARCHIVO
          WTEXT := V_FILA || V_SSEPARADOR_CSV || V_OFICINA || V_SSEPARADOR_CSV || V_OPERACION || V_SSEPARADOR_CSV || V_INTERNO || V_SSEPARADOR_CSV || V_MODALIDAD || V_SSEPARADOR_CSV || V_OPE_UBIGEO ||
                   V_SSEPARADOR_CSV || V_OPE_FECHA || V_SSEPARADOR_CSV || V_OPE_HORA || V_SSEPARADOR_CSV || V_EJE_RELACION || V_SSEPARADOR_CSV || V_EJE_CONDICION || V_SSEPARADOR_CSV || V_EJE_TIPPER ||
                   V_SSEPARADOR_CSV || V_EJE_TIPDOC || V_SSEPARADOR_CSV || V_EJE_NUMDOC || V_SSEPARADOR_CSV || V_EJE_NUMRUC || V_SSEPARADOR_CSV || V_EJE_APEPAT || V_SSEPARADOR_CSV || V_EJE_APEMAT ||
                   V_SSEPARADOR_CSV || V_EJE_NOMBRES || V_SSEPARADOR_CSV || V_EJE_OCUPACION || V_SSEPARADOR_CSV || V_EJE_PAIS || V_SSEPARADOR_CSV || V_EJE_CARGO || V_SSEPARADOR_CSV || V_EJE_PEP ||
                   V_SSEPARADOR_CSV || V_EJE_DOMICILIO || V_SSEPARADOR_CSV || V_EJE_DEPART || V_SSEPARADOR_CSV || V_EJE_PROV || V_SSEPARADOR_CSV || V_EJE_DIST || V_SSEPARADOR_CSV || V_EJE_TELEFONO ||
                   V_SSEPARADOR_CSV || V_ORD_RELACION || V_SSEPARADOR_CSV || V_ORD_CONDICION || V_SSEPARADOR_CSV || V_ORD_TIPPER || V_SSEPARADOR_CSV || V_ORD_TIPDOC || V_SSEPARADOR_CSV ||
                   V_ORD_NUMDOC || V_SSEPARADOR_CSV || V_ORD_NUMRUC || V_SSEPARADOR_CSV || V_ORD_APEPAT || V_SSEPARADOR_CSV || V_ORD_APEMAT || V_SSEPARADOR_CSV || V_ORD_NOMBRES || V_SSEPARADOR_CSV ||
                   V_ORD_OCUPACION || V_SSEPARADOR_CSV || V_ORD_PAIS || V_SSEPARADOR_CSV || V_ORD_CARGO || V_SSEPARADOR_CSV || V_ORD_PEP || V_SSEPARADOR_CSV || V_ORD_DIRECCION || V_SSEPARADOR_CSV ||
                   V_ORD_DOMICILIO || V_SSEPARADOR_CSV || V_UBIGEO || V_SSEPARADOR_CSV || V_ORD_DEPART || V_SSEPARADOR_CSV || V_ORD_PROV || V_SSEPARADOR_CSV || V_ORD_DIST || V_SSEPARADOR_CSV ||
                   V_ORD_TELEFONO || V_SSEPARADOR_CSV || V_BEN_RELACION || V_SSEPARADOR_CSV || V_BEN_CONDICION || V_SSEPARADOR_CSV || V_BEN_TIP_PER || V_SSEPARADOR_CSV || V_BEN_TIP_DOC ||
                   V_SSEPARADOR_CSV || V_BEN_NUM_DOC || V_SSEPARADOR_CSV || V_BEN_NUM_RUC || V_SSEPARADOR_CSV || V_BEN_APEPAT || V_SSEPARADOR_CSV || V_BEN_APEMAT || V_SSEPARADOR_CSV || V_BEN_NOMBRES ||
                   V_SSEPARADOR_CSV || V_BEN_OCUPACION || V_SSEPARADOR_CSV || V_BEN_PAIS || V_SSEPARADOR_CSV || V_BEN_CARGO || V_SSEPARADOR_CSV || V_BEN_PEP || V_SSEPARADOR_CSV || V_SSEPARADOR_CSV ||
                   V_BEN_DOMICILIO || V_SSEPARADOR_CSV || V_SSEPARADOR_CSV || V_BEN_DEPART || V_SSEPARADOR_CSV || V_BEN_PROV || V_SSEPARADOR_CSV || V_BEN_DIST || V_SSEPARADOR_CSV || V_BEN_TELEFONO ||
                   V_SSEPARADOR_CSV || V_DAT_TIPFON || V_SSEPARADOR_CSV || V_DAT_TIPOPE || V_SSEPARADOR_CSV || V_DAT_DESOPE || V_SSEPARADOR_CSV || V_DAT_ORIFON || V_SSEPARADOR_CSV || V_DAT_MONOPE ||
                   V_SSEPARADOR_CSV || V_DAT_MONOPE_A || V_SSEPARADOR_CSV || V_DAT_MTOOPE || V_SSEPARADOR_CSV || V_DAT_MTOOPEA || V_SSEPARADOR_CSV || V_DAT_COD_ENT_INVO || V_SSEPARADOR_CSV ||
                   V_DAT_COD_TIP_CTAO || V_SSEPARADOR_CSV || V_DAT_COD_CTAO || V_SSEPARADOR_CSV || V_DAT_ENT_FNC_EXTO || V_SSEPARADOR_CSV || V_DAT_COD_ENT_INVB || V_SSEPARADOR_CSV ||
                   V_DAT_COD_TIP_CTAB || V_SSEPARADOR_CSV || V_DAT_COD_CTAB || V_SSEPARADOR_CSV || V_DAT_ENT_FNC_EXTB || V_SSEPARADOR_CSV || V_DAT_ALCANCE || V_SSEPARADOR_CSV || V_DAT_COD_PAISO ||
                   V_SSEPARADOR_CSV || V_DAT_COD_PAISD || V_SSEPARADOR_CSV || V_DAT_INTOPE || V_SSEPARADOR_CSV || V_DAT_FORMA || V_SSEPARADOR_CSV || V_DAT_INFORM || V_SSEPARADOR_CSV || V_ORIGEN;

          UTL_FILE.PUT_LINE(FILE_OUTPUT, WTEXT);

        END LOOP;

        RENTAS_MAYORES(P_FECINI, P_FECFIN, P_MONTO, P_TC, V_C_TABLE);

        WTEXT := '';
        SETEAR_VARIABLES;

        LOOP
          FETCH V_C_TABLE
            INTO V_FILA,
                 V_OPERACION,
                 V_OFICINA,
                 V_INTERNO,
                 V_MODALIDAD,
                 V_OPE_UBIGEO,
                 V_OPE_FECHA,
                 V_OPE_HORA,
                 V_EJE_RELACION,
                 V_EJE_CONDICION,
                 V_EJE_TIPPER,
                 V_EJE_TIPDOC,
                 V_EJE_NUMDOC,
                 V_EJE_NUMRUC,
                 V_EJE_APEPAT,
                 V_EJE_APEMAT,
                 V_EJE_NOMBRES,
                 V_EJE_OCUPACION,
                 V_EJE_PAIS,
                 V_EJE_CARGO,
                 V_EJE_PEP,
                 V_EJE_DOMICILIO,
                 V_EJE_DEPART,
                 V_EJE_PROV,
                 V_EJE_DIST,
                 V_EJE_TELEFONO,
                 V_ORD_RELACION,
                 V_ORD_CONDICION,
                 V_ORD_TIPPER,
                 V_ORD_TIPDOC,
                 V_ORD_NUMDOC,
                 V_ORD_NUMRUC,
                 V_ORD_APEPAT,
                 V_ORD_APEMAT,
                 V_ORD_NOMBRES,
                 V_ORD_OCUPACION,
                 V_ORD_PAIS,
                 V_ORD_CARGO,
                 V_ORD_PEP,
                 V_ORD_DOMICILIO,
                 V_ORD_DEPART,
                 V_ORD_PROV,
                 V_ORD_DIST,
                 V_ORD_TELEFONO,
                 V_BEN_RELACION,
                 V_BEN_CONDICION,
                 V_BEN_TIPPER,
                 V_BEN_TIPDOC,
                 V_BEN_NUMDOC,
                 V_BEN_NUMRUC,
                 V_BEN_APEPAT,
                 V_BEN_APEMAT,
                 V_BEN_NOMBRES,
                 V_BEN_OCUPACION,
                 V_BEN_PAIS,
                 V_BEN_CARGO,
                 V_BEN_PEP,
                 V_BEN_DOMICILIO,
                 V_BEN_DEPART,
                 V_BEN_PROV,
                 V_BEN_DIST,
                 V_BEN_TELEFONO,
                 V_DAT_TIPFON,
                 V_DAT_TIPOPE,
                 V_DAT_DESOPE,
                 V_DAT_ORIFON,
                 V_DAT_MONOPE,
                 V_DAT_MONOPEA,
                 V_DAT_MTOOPE,
                 V_DAT_MTOOPEA,
                 V_DAT_COD_ENT_INVO,
                 V_DAT_COD_TIP_CTAO,
                 V_DAT_COD_CTAO,
                 V_DAT_ENT_FNC_EXTO,
                 V_DAT_COD_ENT_INVB,
                 V_DAT_COD_TIP_CTAB,
                 V_DAT_COD_CTAB,
                 V_DAT_ENT_FNC_EXTB,
                 V_DAT_ALCANCE,
                 V_DAT_CODPAISO,
                 V_DAT_CODPAISD,
                 V_DAT_INTERMOPE,
                 V_DAT_FORMA,
                 V_DAT_INFORM,
                 V_TIPO,
                 V_ORIGEN;

          EXIT WHEN V_C_TABLE%NOTFOUND;

          WTEXT := V_FILA || V_SSEPARADOR_CSV || V_OPERACION || V_SSEPARADOR_CSV || V_OFICINA || V_SSEPARADOR_CSV || V_INTERNO || V_SSEPARADOR_CSV || V_MODALIDAD || V_SSEPARADOR_CSV || V_OPE_UBIGEO ||
                   V_SSEPARADOR_CSV || V_OPE_FECHA || V_SSEPARADOR_CSV || V_OPE_HORA || V_SSEPARADOR_CSV || V_EJE_RELACION || V_SSEPARADOR_CSV || V_EJE_CONDICION || V_SSEPARADOR_CSV || V_EJE_TIPPER ||
                   V_SSEPARADOR_CSV || V_EJE_TIPDOC || V_SSEPARADOR_CSV || V_EJE_NUMDOC || V_SSEPARADOR_CSV || V_EJE_NUMRUC || V_SSEPARADOR_CSV || V_EJE_APEPAT || V_SSEPARADOR_CSV || V_EJE_APEMAT ||
                   V_SSEPARADOR_CSV || V_EJE_NOMBRES || V_SSEPARADOR_CSV || V_EJE_OCUPACION || V_SSEPARADOR_CSV || V_EJE_PAIS || V_SSEPARADOR_CSV || V_EJE_CARGO || V_SSEPARADOR_CSV || V_EJE_PEP ||
                   V_SSEPARADOR_CSV || V_EJE_DOMICILIO || V_SSEPARADOR_CSV || V_EJE_DEPART || V_SSEPARADOR_CSV || V_EJE_PROV || V_SSEPARADOR_CSV || V_EJE_DIST || V_SSEPARADOR_CSV || V_EJE_TELEFONO ||
                   V_SSEPARADOR_CSV || V_ORD_RELACION || V_SSEPARADOR_CSV || V_ORD_CONDICION || V_SSEPARADOR_CSV || V_ORD_TIPPER || V_SSEPARADOR_CSV || V_ORD_TIPDOC || V_SSEPARADOR_CSV ||
                   V_ORD_NUMDOC || V_SSEPARADOR_CSV || V_ORD_NUMRUC || V_SSEPARADOR_CSV || V_ORD_APEPAT || V_SSEPARADOR_CSV || V_ORD_APEMAT || V_SSEPARADOR_CSV || V_ORD_NOMBRES || V_SSEPARADOR_CSV ||
                   V_ORD_OCUPACION || V_SSEPARADOR_CSV || V_ORD_PAIS || V_SSEPARADOR_CSV || V_ORD_CARGO || V_SSEPARADOR_CSV || V_ORD_PEP || V_SSEPARADOR_CSV || V_ORD_DOMICILIO || V_SSEPARADOR_CSV ||
                   V_ORD_DEPART || V_SSEPARADOR_CSV || V_ORD_PROV || V_SSEPARADOR_CSV || V_ORD_DIST || V_SSEPARADOR_CSV || V_ORD_TELEFONO || V_SSEPARADOR_CSV || V_BEN_RELACION || V_SSEPARADOR_CSV ||
                   V_BEN_CONDICION || V_SSEPARADOR_CSV || V_BEN_TIPPER || V_SSEPARADOR_CSV || V_BEN_TIPDOC || V_SSEPARADOR_CSV || V_BEN_NUMDOC || V_SSEPARADOR_CSV || V_BEN_NUMRUC || V_SSEPARADOR_CSV ||
                   V_BEN_APEPAT || V_SSEPARADOR_CSV || V_BEN_APEMAT || V_SSEPARADOR_CSV || V_BEN_NOMBRES || V_SSEPARADOR_CSV || V_BEN_OCUPACION || V_SSEPARADOR_CSV || V_BEN_PAIS || V_SSEPARADOR_CSV ||
                   V_BEN_CARGO || V_SSEPARADOR_CSV || V_BEN_PEP || V_SSEPARADOR_CSV || V_BEN_DOMICILIO || V_SSEPARADOR_CSV || V_BEN_DEPART || V_SSEPARADOR_CSV || V_BEN_PROV || V_SSEPARADOR_CSV ||
                   V_BEN_DIST || V_SSEPARADOR_CSV || V_BEN_TELEFONO || V_SSEPARADOR_CSV || V_DAT_TIPFON || V_SSEPARADOR_CSV || V_DAT_TIPOPE || V_SSEPARADOR_CSV || V_DAT_DESOPE || V_SSEPARADOR_CSV ||
                   V_DAT_ORIFON || V_SSEPARADOR_CSV || V_DAT_MONOPE || V_SSEPARADOR_CSV || V_DAT_MONOPEA || V_SSEPARADOR_CSV || V_DAT_MTOOPE || V_SSEPARADOR_CSV || V_DAT_MTOOPEA || V_SSEPARADOR_CSV ||
                   V_DAT_COD_ENT_INVO || V_SSEPARADOR_CSV || V_DAT_COD_TIP_CTAO || V_SSEPARADOR_CSV || V_DAT_COD_CTAO || V_SSEPARADOR_CSV || V_DAT_ENT_FNC_EXTO || V_SSEPARADOR_CSV ||
                   V_DAT_COD_ENT_INVB || V_SSEPARADOR_CSV || V_DAT_COD_TIP_CTAB || V_SSEPARADOR_CSV || V_DAT_COD_CTAB || V_SSEPARADOR_CSV || V_DAT_ENT_FNC_EXTB || V_SSEPARADOR_CSV || V_DAT_ALCANCE ||
                   V_SSEPARADOR_CSV || V_DAT_CODPAISO || V_SSEPARADOR_CSV || V_DAT_CODPAISD || V_SSEPARADOR_CSV || V_DAT_INTERMOPE || V_SSEPARADOR_CSV || V_DAT_FORMA || V_SSEPARADOR_CSV ||
                   V_DAT_INFORM || V_SSEPARADOR_CSV || V_TIPO || V_SSEPARADOR_CSV || V_ORIGEN;

          UTL_FILE.PUT_LINE(FILE_OUTPUT, WTEXT);
        END LOOP;

      ELSIF P_EST_REPORTES = 'SVR' THEN

        SINIESTROS_MAYORES(P_OPE, P_TC, P_MONTO, P_FECINI, P_FECFIN, V_C_TABLE);

        WTEXT := '';
        SETEAR_VARIABLES;

        LOOP
          FETCH V_C_TABLE

            INTO V_FILA,
                 V_OFICINA,
                 V_OPERACION,
                 V_INTERNO,
                 V_MODALIDAD,
                 V_OPE_UBIGEO,
                 V_OPE_FECHA,
                 V_OPE_HORA,
                 V_EJE_RELACION,
                 V_EJE_CONDICION,
                 V_EJE_TIPPER,
                 V_EJE_TIPDOC,
                 V_EJE_NUMDOC,
                 V_EJE_NUMRUC,
                 V_EJE_APEPAT,
                 V_EJE_APEMAT,
                 V_EJE_NOMBRES,
                 V_EJE_OCUPACION,
                 V_EJE_PAIS,
                 V_EJE_CARGO,
                 V_EJE_PEP,
                 V_EJE_DOMICILIO,
                 V_EJE_DEPART,
                 V_EJE_PROV,
                 V_EJE_DIST,
                 V_EJE_TELEFONO,
                 V_ORD_RELACION,
                 V_ORD_CONDICION,
                 V_ORD_TIPPER,
                 V_ORD_TIPDOC,
                 V_ORD_NUMDOC,
                 V_ORD_NUMRUC,
                 V_ORD_APEPAT,
                 V_ORD_APEMAT,
                 V_ORD_NOMBRES,
                 V_ORD_OCUPACION,
                 V_ORD_PAIS,
                 V_ORD_CARGO,
                 V_ORD_PEP,
                 V_ORD_DOMICILIO,
                 V_ORD_DEPART,
                 V_ORD_PROV,
                 V_ORD_DIST,
                 V_ORD_TELEFONO,
                 V_BEN_RELACION,
                 V_BEN_CONDICION,
                 V_BEN_TIP_PER,
                 V_BEN_TIP_DOC,
                 V_BEN_NUM_DOC,
                 V_BEN_NUM_RUC,
                 V_BEN_APEPAT,
                 V_BEN_APEMAT,
                 V_BEN_NOMBRES,
                 V_BEN_OCUPACION,
                 V_BEN_PAIS,
                 V_BEN_CARGO,
                 V_BEN_PEP,

                 V_BEN_DOMICILIO,

                 V_BEN_DEPART,
                 V_BEN_PROV,
                 V_BEN_DIST,
                 V_BEN_TELEFONO,
                 V_DAT_TIPFON,
                 V_DAT_TIPOPE,
                 V_DAT_DESOPE,
                 V_DAT_ORIFON,
                 V_DAT_MONOPE,
                 V_DAT_MONOPE_A,
                 V_DAT_MTOOPE,
                 V_DAT_MTOOPEA,
                 V_DAT_COD_ENT_INVO,
                 V_DAT_COD_TIP_CTAO,
                 V_DAT_COD_CTAO,
                 V_DAT_ENT_FNC_EXTO,
                 V_DAT_COD_ENT_INVB,
                 V_DAT_COD_TIP_CTAB,
                 V_DAT_COD_CTAB,
                 V_DAT_ENT_FNC_EXTB,
                 V_DAT_ALCANCE,
                 V_DAT_COD_PAISO,
                 V_DAT_COD_PAISD,
                 V_DAT_INTOPE,
                 V_DAT_FORMA,
                 V_DAT_INFORM,
                 V_ORIGEN;

          EXIT WHEN V_C_TABLE%NOTFOUND;

          --CUERPO DEL ARCHIVO
          WTEXT := V_FILA || V_SSEPARADOR_CSV || V_OFICINA || V_SSEPARADOR_CSV || V_OPERACION || V_SSEPARADOR_CSV || V_INTERNO || V_SSEPARADOR_CSV || V_MODALIDAD || V_SSEPARADOR_CSV || V_OPE_UBIGEO ||
                   V_SSEPARADOR_CSV || V_OPE_FECHA || V_SSEPARADOR_CSV || V_OPE_HORA || V_SSEPARADOR_CSV || V_EJE_RELACION || V_SSEPARADOR_CSV || V_EJE_CONDICION || V_SSEPARADOR_CSV || V_EJE_TIPPER ||
                   V_SSEPARADOR_CSV || V_EJE_TIPDOC || V_SSEPARADOR_CSV || V_EJE_NUMDOC || V_SSEPARADOR_CSV || V_EJE_NUMRUC || V_SSEPARADOR_CSV || V_EJE_APEPAT || V_SSEPARADOR_CSV || V_EJE_APEMAT ||
                   V_SSEPARADOR_CSV || V_EJE_NOMBRES || V_SSEPARADOR_CSV || V_EJE_OCUPACION || V_SSEPARADOR_CSV || V_EJE_PAIS || V_SSEPARADOR_CSV || V_EJE_CARGO || V_SSEPARADOR_CSV || V_EJE_PEP ||
                   V_SSEPARADOR_CSV || V_EJE_DOMICILIO || V_SSEPARADOR_CSV || V_EJE_DEPART || V_SSEPARADOR_CSV || V_EJE_PROV || V_SSEPARADOR_CSV || V_EJE_DIST || V_SSEPARADOR_CSV || V_EJE_TELEFONO ||
                   V_SSEPARADOR_CSV || V_ORD_RELACION || V_SSEPARADOR_CSV || V_ORD_CONDICION || V_SSEPARADOR_CSV || V_ORD_TIPPER || V_SSEPARADOR_CSV || V_ORD_TIPDOC || V_SSEPARADOR_CSV ||
                   V_ORD_NUMDOC || V_SSEPARADOR_CSV || V_ORD_NUMRUC || V_SSEPARADOR_CSV || V_ORD_APEPAT || V_SSEPARADOR_CSV || V_ORD_APEMAT || V_SSEPARADOR_CSV || V_ORD_NOMBRES || V_SSEPARADOR_CSV ||
                   V_ORD_OCUPACION || V_SSEPARADOR_CSV || V_ORD_PAIS || V_SSEPARADOR_CSV || V_ORD_CARGO || V_SSEPARADOR_CSV || V_ORD_PEP || V_SSEPARADOR_CSV || V_ORD_DOMICILIO || V_SSEPARADOR_CSV ||
                   V_ORD_DEPART || V_SSEPARADOR_CSV || V_ORD_PROV || V_SSEPARADOR_CSV || V_ORD_DIST || V_SSEPARADOR_CSV || V_ORD_TELEFONO || V_SSEPARADOR_CSV || V_BEN_RELACION || V_SSEPARADOR_CSV ||
                   V_BEN_CONDICION || V_SSEPARADOR_CSV || V_BEN_TIP_PER || V_SSEPARADOR_CSV || V_BEN_TIP_DOC || V_SSEPARADOR_CSV || V_BEN_NUM_DOC || V_SSEPARADOR_CSV || V_BEN_NUM_RUC ||
                   V_SSEPARADOR_CSV || V_BEN_APEPAT || V_SSEPARADOR_CSV || V_BEN_APEMAT || V_SSEPARADOR_CSV || V_BEN_NOMBRES || V_SSEPARADOR_CSV || V_BEN_OCUPACION || V_SSEPARADOR_CSV || V_BEN_PAIS ||
                   V_SSEPARADOR_CSV || V_BEN_CARGO || V_SSEPARADOR_CSV || V_BEN_PEP || V_SSEPARADOR_CSV || V_BEN_DOMICILIO || V_SSEPARADOR_CSV || V_BEN_DEPART || V_SSEPARADOR_CSV || V_BEN_PROV ||
                   V_SSEPARADOR_CSV || V_BEN_DIST || V_SSEPARADOR_CSV || V_BEN_TELEFONO || V_SSEPARADOR_CSV || V_DAT_TIPFON || V_SSEPARADOR_CSV || V_DAT_TIPOPE || V_SSEPARADOR_CSV || V_DAT_DESOPE ||
                   V_SSEPARADOR_CSV || V_DAT_ORIFON || V_SSEPARADOR_CSV || V_DAT_MONOPE || V_SSEPARADOR_CSV || V_DAT_MONOPE_A || V_SSEPARADOR_CSV || V_DAT_MTOOPE || V_SSEPARADOR_CSV || V_DAT_MTOOPEA ||
                   V_SSEPARADOR_CSV || V_DAT_COD_ENT_INVO || V_SSEPARADOR_CSV || V_DAT_COD_TIP_CTAO || V_SSEPARADOR_CSV || V_DAT_COD_CTAO || V_SSEPARADOR_CSV || V_DAT_ENT_FNC_EXTO || V_SSEPARADOR_CSV ||
                   V_DAT_COD_ENT_INVB || V_SSEPARADOR_CSV || V_DAT_COD_TIP_CTAB || V_SSEPARADOR_CSV || V_DAT_COD_CTAB || V_SSEPARADOR_CSV || V_DAT_ENT_FNC_EXTB || V_SSEPARADOR_CSV || V_DAT_ALCANCE ||
                   V_SSEPARADOR_CSV || V_DAT_COD_PAISO || V_SSEPARADOR_CSV || V_DAT_COD_PAISD || V_SSEPARADOR_CSV || V_DAT_INTOPE || V_SSEPARADOR_CSV || V_DAT_FORMA || V_SSEPARADOR_CSV ||
                   V_DAT_INFORM || V_SSEPARADOR_CSV || V_ORIGEN;

          UTL_FILE.PUT_LINE(FILE_OUTPUT, WTEXT);
        END LOOP;

        RENTAS_MAYORES(P_FECINI, P_FECFIN, P_MONTO, P_TC, V_C_TABLE);

        WTEXT := '';
        SETEAR_VARIABLES;

        LOOP
          FETCH V_C_TABLE
            INTO V_FILA,
                 V_OPERACION,
                 V_OFICINA,
                 V_INTERNO,
                 V_MODALIDAD,
                 V_OPE_UBIGEO,
                 V_OPE_FECHA,
                 V_OPE_HORA,
                 V_EJE_RELACION,
                 V_EJE_CONDICION,
                 V_EJE_TIPPER,
                 V_EJE_TIPDOC,
                 V_EJE_NUMDOC,
                 V_EJE_NUMRUC,
                 V_EJE_APEPAT,
                 V_EJE_APEMAT,
                 V_EJE_NOMBRES,
                 V_EJE_OCUPACION,
                 V_EJE_PAIS,
                 V_EJE_CARGO,
                 V_EJE_PEP,
                 V_EJE_DOMICILIO,
                 V_EJE_DEPART,
                 V_EJE_PROV,
                 V_EJE_DIST,
                 V_EJE_TELEFONO,
                 V_ORD_RELACION,
                 V_ORD_CONDICION,
                 V_ORD_TIPPER,
                 V_ORD_TIPDOC,
                 V_ORD_NUMDOC,
                 V_ORD_NUMRUC,
                 V_ORD_APEPAT,
                 V_ORD_APEMAT,
                 V_ORD_NOMBRES,
                 V_ORD_OCUPACION,
                 V_ORD_PAIS,
                 V_ORD_CARGO,
                 V_ORD_PEP,
                 V_ORD_DOMICILIO,
                 V_ORD_DEPART,
                 V_ORD_PROV,
                 V_ORD_DIST,
                 V_ORD_TELEFONO,
                 V_BEN_RELACION,
                 V_BEN_CONDICION,
                 V_BEN_TIPPER,
                 V_BEN_TIPDOC,
                 V_BEN_NUMDOC,
                 V_BEN_NUMRUC,
                 V_BEN_APEPAT,
                 V_BEN_APEMAT,
                 V_BEN_NOMBRES,
                 V_BEN_OCUPACION,
                 V_BEN_PAIS,
                 V_BEN_CARGO,
                 V_BEN_PEP,
                 V_BEN_DOMICILIO,
                 V_BEN_DEPART,
                 V_BEN_PROV,
                 V_BEN_DIST,
                 V_BEN_TELEFONO,
                 V_DAT_TIPFON,
                 V_DAT_TIPOPE,
                 V_DAT_DESOPE,
                 V_DAT_ORIFON,
                 V_DAT_MONOPE,
                 V_DAT_MONOPEA,
                 V_DAT_MTOOPE,
                 V_DAT_MTOOPEA,
                 V_DAT_COD_ENT_INVO,
                 V_DAT_COD_TIP_CTAO,
                 V_DAT_COD_CTAO,
                 V_DAT_ENT_FNC_EXTO,
                 V_DAT_COD_ENT_INVB,
                 V_DAT_COD_TIP_CTAB,
                 V_DAT_COD_CTAB,
                 V_DAT_ENT_FNC_EXTB,
                 V_DAT_ALCANCE,
                 V_DAT_CODPAISO,
                 V_DAT_CODPAISD,
                 V_DAT_INTERMOPE,
                 V_DAT_FORMA,
                 V_DAT_INFORM,
                 V_TIPO,
                 V_ORIGEN;

          EXIT WHEN V_C_TABLE%NOTFOUND;

          WTEXT := V_FILA || V_SSEPARADOR_CSV || V_OPERACION || V_SSEPARADOR_CSV || V_OFICINA || V_SSEPARADOR_CSV || V_INTERNO || V_SSEPARADOR_CSV || V_MODALIDAD || V_SSEPARADOR_CSV || V_OPE_UBIGEO ||
                   V_SSEPARADOR_CSV || V_OPE_FECHA || V_SSEPARADOR_CSV || V_OPE_HORA || V_SSEPARADOR_CSV || V_EJE_RELACION || V_SSEPARADOR_CSV || V_EJE_CONDICION || V_SSEPARADOR_CSV || V_EJE_TIPPER ||
                   V_SSEPARADOR_CSV || V_EJE_TIPDOC || V_SSEPARADOR_CSV || V_EJE_NUMDOC || V_SSEPARADOR_CSV || V_EJE_NUMRUC || V_SSEPARADOR_CSV || V_EJE_APEPAT || V_SSEPARADOR_CSV || V_EJE_APEMAT ||
                   V_SSEPARADOR_CSV || V_EJE_NOMBRES || V_SSEPARADOR_CSV || V_EJE_OCUPACION || V_SSEPARADOR_CSV || V_EJE_PAIS || V_SSEPARADOR_CSV || V_EJE_CARGO || V_SSEPARADOR_CSV || V_EJE_PEP ||
                   V_SSEPARADOR_CSV || V_EJE_DOMICILIO || V_SSEPARADOR_CSV || V_EJE_DEPART || V_SSEPARADOR_CSV || V_EJE_PROV || V_SSEPARADOR_CSV || V_EJE_DIST || V_SSEPARADOR_CSV || V_EJE_TELEFONO ||
                   V_SSEPARADOR_CSV || V_ORD_RELACION || V_SSEPARADOR_CSV || V_ORD_CONDICION || V_SSEPARADOR_CSV || V_ORD_TIPPER || V_SSEPARADOR_CSV || V_ORD_TIPDOC || V_SSEPARADOR_CSV ||
                   V_ORD_NUMDOC || V_SSEPARADOR_CSV || V_ORD_NUMRUC || V_SSEPARADOR_CSV || V_ORD_APEPAT || V_SSEPARADOR_CSV || V_ORD_APEMAT || V_SSEPARADOR_CSV || V_ORD_NOMBRES || V_SSEPARADOR_CSV ||
                   V_ORD_OCUPACION || V_SSEPARADOR_CSV || V_ORD_PAIS || V_SSEPARADOR_CSV || V_ORD_CARGO || V_SSEPARADOR_CSV || V_ORD_PEP || V_SSEPARADOR_CSV || V_ORD_DOMICILIO || V_SSEPARADOR_CSV ||
                   V_ORD_DEPART || V_SSEPARADOR_CSV || V_ORD_PROV || V_SSEPARADOR_CSV || V_ORD_DIST || V_SSEPARADOR_CSV || V_ORD_TELEFONO || V_SSEPARADOR_CSV || V_BEN_RELACION || V_SSEPARADOR_CSV ||
                   V_BEN_CONDICION || V_SSEPARADOR_CSV || V_BEN_TIPPER || V_SSEPARADOR_CSV || V_BEN_TIPDOC || V_SSEPARADOR_CSV || V_BEN_NUMDOC || V_SSEPARADOR_CSV || V_BEN_NUMRUC || V_SSEPARADOR_CSV ||
                   V_BEN_APEPAT || V_SSEPARADOR_CSV || V_BEN_APEMAT || V_SSEPARADOR_CSV || V_BEN_NOMBRES || V_SSEPARADOR_CSV || V_BEN_OCUPACION || V_SSEPARADOR_CSV || V_BEN_PAIS || V_SSEPARADOR_CSV ||
                   V_BEN_CARGO || V_SSEPARADOR_CSV || V_BEN_PEP || V_SSEPARADOR_CSV || V_BEN_DOMICILIO || V_SSEPARADOR_CSV || V_BEN_DEPART || V_SSEPARADOR_CSV || V_BEN_PROV || V_SSEPARADOR_CSV ||
                   V_BEN_DIST || V_SSEPARADOR_CSV || V_BEN_TELEFONO || V_SSEPARADOR_CSV || V_DAT_TIPFON || V_SSEPARADOR_CSV || V_DAT_TIPOPE || V_SSEPARADOR_CSV || V_DAT_DESOPE || V_SSEPARADOR_CSV ||
                   V_DAT_ORIFON || V_SSEPARADOR_CSV || V_DAT_MONOPE || V_SSEPARADOR_CSV || V_DAT_MONOPEA || V_SSEPARADOR_CSV || V_DAT_MTOOPE || V_SSEPARADOR_CSV || V_DAT_MTOOPEA || V_SSEPARADOR_CSV ||
                   V_DAT_COD_ENT_INVO || V_SSEPARADOR_CSV || V_DAT_COD_TIP_CTAO || V_SSEPARADOR_CSV || V_DAT_COD_CTAO || V_SSEPARADOR_CSV || V_DAT_ENT_FNC_EXTO || V_SSEPARADOR_CSV ||
                   V_DAT_COD_ENT_INVB || V_SSEPARADOR_CSV || V_DAT_COD_TIP_CTAB || V_SSEPARADOR_CSV || V_DAT_COD_CTAB || V_SSEPARADOR_CSV || V_DAT_ENT_FNC_EXTB || V_SSEPARADOR_CSV || V_DAT_ALCANCE ||
                   V_SSEPARADOR_CSV || V_DAT_CODPAISO || V_SSEPARADOR_CSV || V_DAT_CODPAISD || V_SSEPARADOR_CSV || V_DAT_INTERMOPE || V_SSEPARADOR_CSV || V_DAT_FORMA || V_SSEPARADOR_CSV ||
                   V_DAT_INFORM || V_SSEPARADOR_CSV || V_TIPO || V_SSEPARADOR_CSV || V_ORIGEN;

          UTL_FILE.PUT_LINE(FILE_OUTPUT, WTEXT);
        END LOOP;

        VIDA_MAYORES(P_OPE, P_TC, P_MONTO, P_FECINI, P_FECFIN, V_C_TABLE);

        WTEXT := '';
        SETEAR_VARIABLES;

        LOOP
          FETCH V_C_TABLE
            INTO V_FILA,
                 V_OFICINA,
                 V_OPERACION,
                 V_INTERNO,
                 V_MODALIDAD,
                 V_OPE_UBIGEO,
                 V_OPE_FECHA,
                 V_OPE_HORA,
                 V_EJE_RELACION,
                 V_EJE_CONDICION,
                 V_EJE_TIPPER,
                 V_EJE_TIPDOC,
                 V_EJE_NUMDOC,
                 V_EJE_NUMRUC,
                 V_EJE_APEPAT,
                 V_EJE_APEMAT,
                 V_EJE_NOMBRES,
                 V_EJE_OCUPACION,
                 V_EJE_PAIS,
                 V_EJE_CARGO,
                 V_EJE_PEP,
                 V_EJE_DOMICILIO,
                 V_EJE_DEPART,
                 V_EJE_PROV,
                 V_EJE_DIST,
                 V_EJE_TELEFONO,
                 V_ORD_RELACION,
                 V_ORD_CONDICION,
                 V_ORD_TIPPER,
                 V_ORD_TIPDOC,
                 V_ORD_NUMDOC,
                 V_ORD_NUMRUC,
                 V_ORD_APEPAT,
                 V_ORD_APEMAT,
                 V_ORD_NOMBRES,
                 V_ORD_OCUPACION,
                 V_ORD_PAIS,
                 V_ORD_CARGO,
                 V_ORD_PEP,
                 V_ORD_DOMICILIO,
                 V_ORD_DEPART,
                 V_ORD_PROV,
                 V_ORD_DIST,
                 V_ORD_TELEFONO,
                 V_BEN_RELACION,
                 V_BEN_CONDICION,
                 V_BEN_TIP_PER,
                 V_BEN_TIP_DOC,
                 V_BEN_NUM_DOC,
                 V_BEN_NUM_RUC,
                 V_BEN_APEPAT,
                 V_BEN_APEMAT,
                 V_BEN_NOMBRES,
                 V_BEN_OCUPACION,
                 V_BEN_PAIS,
                 V_BEN_CARGO,
                 V_BEN_PEP,
                 V_BEN_DOMICILIO,
                 V_BEN_DEPART,
                 V_BEN_PROV,
                 V_BEN_DIST,
                 V_BEN_TELEFONO,
                 V_DAT_TIPFON,
                 V_DAT_TIPOPE,
                 V_DAT_DESOPE,
                 V_DAT_ORIFON,
                 V_DAT_MONOPE,
                 V_DAT_MONOPE_A,
                 V_DAT_MTOOPE,
                 V_DAT_MTOOPEA,
                 V_DAT_COD_ENT_INVO,
                 V_DAT_COD_TIP_CTAO,
                 V_DAT_COD_CTAO,
                 V_DAT_ENT_FNC_EXTO,
                 V_DAT_COD_ENT_INVB,
                 V_DAT_COD_TIP_CTAB,
                 V_DAT_COD_CTAB,
                 V_DAT_ENT_FNC_EXTB,
                 V_DAT_ALCANCE,
                 V_DAT_COD_PAISO,
                 V_DAT_COD_PAISD,
                 V_DAT_INTOPE,
                 V_DAT_FORMA,
                 V_DAT_INFORM,
                 V_ORIGEN;

          EXIT WHEN V_C_TABLE%NOTFOUND;

          --CUERPO DEL ARCHIVO
          WTEXT := V_FILA || V_SSEPARADOR_CSV || V_OFICINA || V_SSEPARADOR_CSV || V_OPERACION || V_SSEPARADOR_CSV || V_INTERNO || V_SSEPARADOR_CSV || V_MODALIDAD || V_SSEPARADOR_CSV || V_OPE_UBIGEO ||
                   V_SSEPARADOR_CSV || V_OPE_FECHA || V_SSEPARADOR_CSV || V_OPE_HORA || V_SSEPARADOR_CSV || V_EJE_RELACION || V_SSEPARADOR_CSV || V_EJE_CONDICION || V_SSEPARADOR_CSV || V_EJE_TIPPER ||
                   V_SSEPARADOR_CSV || V_EJE_TIPDOC || V_SSEPARADOR_CSV || V_EJE_NUMDOC || V_SSEPARADOR_CSV || V_EJE_NUMRUC || V_SSEPARADOR_CSV || V_EJE_APEPAT || V_SSEPARADOR_CSV || V_EJE_APEMAT ||
                   V_SSEPARADOR_CSV || V_EJE_NOMBRES || V_SSEPARADOR_CSV || V_EJE_OCUPACION || V_SSEPARADOR_CSV || V_EJE_PAIS || V_SSEPARADOR_CSV || V_EJE_CARGO || V_SSEPARADOR_CSV || V_EJE_PEP ||
                   V_SSEPARADOR_CSV || V_EJE_DOMICILIO || V_SSEPARADOR_CSV || V_EJE_DEPART || V_SSEPARADOR_CSV || V_EJE_PROV || V_SSEPARADOR_CSV || V_EJE_DIST || V_SSEPARADOR_CSV || V_EJE_TELEFONO ||
                   V_SSEPARADOR_CSV || V_ORD_RELACION || V_SSEPARADOR_CSV || V_ORD_CONDICION || V_SSEPARADOR_CSV || V_ORD_TIPPER || V_SSEPARADOR_CSV || V_ORD_TIPDOC || V_SSEPARADOR_CSV ||
                   V_ORD_NUMDOC || V_SSEPARADOR_CSV || V_ORD_NUMRUC || V_SSEPARADOR_CSV || V_ORD_APEPAT || V_SSEPARADOR_CSV || V_ORD_APEMAT || V_SSEPARADOR_CSV || V_ORD_NOMBRES || V_SSEPARADOR_CSV ||
                   V_ORD_OCUPACION || V_SSEPARADOR_CSV || V_ORD_PAIS || V_SSEPARADOR_CSV || V_ORD_CARGO || V_SSEPARADOR_CSV || V_ORD_PEP || V_SSEPARADOR_CSV || V_ORD_DOMICILIO || V_SSEPARADOR_CSV ||
                   V_ORD_DEPART || V_SSEPARADOR_CSV || V_ORD_PROV || V_SSEPARADOR_CSV || V_ORD_DIST || V_SSEPARADOR_CSV || V_ORD_TELEFONO || V_SSEPARADOR_CSV || V_BEN_RELACION || V_SSEPARADOR_CSV ||
                   V_BEN_CONDICION || V_SSEPARADOR_CSV || V_BEN_TIP_PER || V_SSEPARADOR_CSV || V_BEN_TIP_DOC || V_SSEPARADOR_CSV || V_BEN_NUM_DOC || V_SSEPARADOR_CSV || V_BEN_NUM_RUC ||
                   V_SSEPARADOR_CSV || V_BEN_APEPAT || V_SSEPARADOR_CSV || V_BEN_APEMAT || V_SSEPARADOR_CSV || V_BEN_NOMBRES || V_SSEPARADOR_CSV || V_BEN_OCUPACION || V_SSEPARADOR_CSV || V_BEN_PAIS ||
                   V_SSEPARADOR_CSV || V_BEN_CARGO || V_SSEPARADOR_CSV || V_BEN_PEP || V_SSEPARADOR_CSV || V_BEN_DOMICILIO || V_SSEPARADOR_CSV || V_BEN_DEPART || V_SSEPARADOR_CSV || V_BEN_PROV ||
                   V_SSEPARADOR_CSV || V_BEN_DIST || V_SSEPARADOR_CSV || V_BEN_TELEFONO || V_SSEPARADOR_CSV || V_DAT_TIPFON || V_SSEPARADOR_CSV || V_DAT_TIPOPE || V_SSEPARADOR_CSV || V_DAT_DESOPE ||
                   V_SSEPARADOR_CSV || V_DAT_ORIFON || V_SSEPARADOR_CSV || V_DAT_MONOPE || V_SSEPARADOR_CSV || V_DAT_MONOPE_A || V_SSEPARADOR_CSV || V_DAT_MTOOPE || V_SSEPARADOR_CSV || V_DAT_MTOOPEA ||
                   V_SSEPARADOR_CSV || V_DAT_COD_ENT_INVO || V_SSEPARADOR_CSV || V_DAT_COD_TIP_CTAO || V_SSEPARADOR_CSV || V_DAT_COD_CTAO || V_SSEPARADOR_CSV || V_DAT_ENT_FNC_EXTO || V_SSEPARADOR_CSV ||
                   V_DAT_COD_ENT_INVB || V_SSEPARADOR_CSV || V_DAT_COD_TIP_CTAB || V_SSEPARADOR_CSV || V_DAT_COD_CTAB || V_SSEPARADOR_CSV || V_DAT_ENT_FNC_EXTB || V_SSEPARADOR_CSV || V_DAT_ALCANCE ||
                   V_SSEPARADOR_CSV || V_DAT_COD_PAISO || V_SSEPARADOR_CSV || V_DAT_COD_PAISD || V_SSEPARADOR_CSV || V_DAT_INTOPE || V_SSEPARADOR_CSV || V_DAT_FORMA || V_SSEPARADOR_CSV ||
                   V_DAT_INFORM || V_SSEPARADOR_CSV || V_ORIGEN;

          UTL_FILE.PUT_LINE(FILE_OUTPUT, WTEXT);

        END LOOP;

      END IF;

      UTL_FILE.FCLOSE(FILE_OUTPUT);

      P_SRUTA := '\\' || P_SRUTA || '\' || WRUTA || '\' || WNOMARCH;

      CLOSE V_C_TABLE;

    ELSIF P_TIPO_ARCHIVO = 'txt' THEN

      --CABECERA PARA TXT
      WTEXT := '0501' || V_SSEPARADOR_TXT || '01' || V_SSEPARADOR_TXT || 'REVIS' || V_SSEPARADOR_TXT || TO_CHAR(SYSDATE, 'YYYYMMDD') || V_SSEPARADOR_TXT || '012' || V_SSEPARADOR_TXT ||
               '               ';

      --TERMINA LA LÍNEA DE CABECERA
      UTL_FILE.PUT_LINE(FILE_OUTPUT, WTEXT);

      --DETALLE PARA TXT
      IF P_EST_REPORTES = 'S' THEN

        SINIESTROS_MAYORES(P_OPE, P_TC, P_MONTO, P_FECINI, P_FECFIN, V_C_TABLE);

        WTEXT := '';
        SETEAR_VARIABLES;

        LOOP
          FETCH V_C_TABLE

            INTO V_FILA,
                 V_OFICINA,
                 V_OPERACION,
                 V_INTERNO,
                 V_MODALIDAD,
                 V_OPE_UBIGEO,
                 V_OPE_FECHA,
                 V_OPE_HORA,
                 V_EJE_RELACION,
                 V_EJE_CONDICION,
                 V_EJE_TIPPER,
                 V_EJE_TIPDOC,
                 V_EJE_NUMDOC,
                 V_EJE_NUMRUC,
                 V_EJE_APEPAT,
                 V_EJE_APEMAT,
                 V_EJE_NOMBRES,
                 V_EJE_OCUPACION,
                 V_EJE_PAIS,
                 V_EJE_CARGO,
                 V_EJE_PEP,
                 V_EJE_DOMICILIO,
                 V_EJE_DEPART,
                 V_EJE_PROV,
                 V_EJE_DIST,
                 V_EJE_TELEFONO,
                 V_ORD_RELACION,
                 V_ORD_CONDICION,
                 V_ORD_TIPPER,
                 V_ORD_TIPDOC,
                 V_ORD_NUMDOC,
                 V_ORD_NUMRUC,
                 V_ORD_APEPAT,
                 V_ORD_APEMAT,
                 V_ORD_NOMBRES,
                 V_ORD_OCUPACION,
                 V_ORD_PAIS,
                 V_ORD_CARGO,
                 V_ORD_PEP,
                 V_ORD_DOMICILIO,
                 V_ORD_DEPART,
                 V_ORD_PROV,
                 V_ORD_DIST,

                 V_ORD_TELEFONO,
                 V_BEN_RELACION,
                 V_BEN_CONDICION,
                 V_BEN_TIP_PER,
                 V_BEN_TIP_DOC,
                 V_BEN_NUM_DOC,
                 V_BEN_NUM_RUC,
                 V_BEN_APEPAT,
                 V_BEN_APEMAT,
                 V_BEN_NOMBRES,
                 V_BEN_OCUPACION,
                 V_BEN_PAIS,
                 V_BEN_CARGO,
                 V_BEN_PEP,

                 V_BEN_DOMICILIO,

                 V_BEN_DEPART,
                 V_BEN_PROV,
                 V_BEN_DIST,
                 V_BEN_TELEFONO,
                 V_DAT_TIPFON,
                 V_DAT_TIPOPE,
                 V_DAT_DESOPE,
                 V_DAT_ORIFON,
                 V_DAT_MONOPE,
                 V_DAT_MONOPE_A,
                 V_DAT_MTOOPE,
                 V_DAT_MTOOPEA,
                 V_DAT_COD_ENT_INVO,
                 V_DAT_COD_TIP_CTAO,
                 V_DAT_COD_CTAO,
                 V_DAT_ENT_FNC_EXTO,
                 V_DAT_COD_ENT_INVB,
                 V_DAT_COD_TIP_CTAB,
                 V_DAT_COD_CTAB,
                 V_DAT_ENT_FNC_EXTB,
                 V_DAT_ALCANCE,
                 V_DAT_COD_PAISO,
                 V_DAT_COD_PAISD,
                 V_DAT_INTOPE,
                 V_DAT_FORMA,
                 V_DAT_INFORM,
                 V_ORIGEN;

          EXIT WHEN V_C_TABLE%NOTFOUND;

          --CUERPO DEL ARCHIVO
          WTEXT := V_FILA || V_SSEPARADOR_TXT || V_OFICINA || V_SSEPARADOR_TXT || V_OPERACION || V_SSEPARADOR_TXT || V_INTERNO || V_SSEPARADOR_TXT || V_MODALIDAD || V_SSEPARADOR_TXT || V_OPE_UBIGEO ||
                   V_SSEPARADOR_TXT || V_OPE_FECHA || V_SSEPARADOR_TXT || V_OPE_HORA || V_SSEPARADOR_TXT || V_EJE_RELACION || V_SSEPARADOR_TXT || V_EJE_CONDICION || V_SSEPARADOR_TXT || V_EJE_TIPPER ||
                   V_SSEPARADOR_TXT || V_EJE_TIPDOC || V_SSEPARADOR_TXT || V_EJE_NUMDOC || V_SSEPARADOR_TXT || V_EJE_NUMRUC || V_SSEPARADOR_TXT || V_EJE_APEPAT || V_SSEPARADOR_TXT || V_EJE_APEMAT ||
                   V_SSEPARADOR_TXT || V_EJE_NOMBRES || V_SSEPARADOR_TXT || V_EJE_OCUPACION || V_SSEPARADOR_TXT || V_EJE_PAIS || V_SSEPARADOR_TXT || V_EJE_CARGO || V_SSEPARADOR_TXT || V_EJE_PEP ||
                   V_SSEPARADOR_TXT || V_EJE_DOMICILIO || V_SSEPARADOR_TXT || V_EJE_DEPART || V_SSEPARADOR_TXT || V_EJE_PROV || V_SSEPARADOR_TXT || V_EJE_DIST || V_SSEPARADOR_TXT || V_EJE_TELEFONO ||
                   V_SSEPARADOR_TXT || V_ORD_RELACION || V_SSEPARADOR_TXT || V_ORD_CONDICION || V_SSEPARADOR_TXT || V_ORD_TIPPER || V_SSEPARADOR_TXT || V_ORD_TIPDOC || V_SSEPARADOR_TXT ||
                   V_ORD_NUMDOC || V_SSEPARADOR_TXT || V_ORD_NUMRUC || V_SSEPARADOR_TXT || V_ORD_APEPAT || V_SSEPARADOR_TXT || V_ORD_APEMAT || V_SSEPARADOR_TXT || V_ORD_NOMBRES || V_SSEPARADOR_TXT ||
                   V_ORD_OCUPACION || V_SSEPARADOR_TXT || V_ORD_PAIS || V_SSEPARADOR_TXT || V_ORD_CARGO || V_SSEPARADOR_TXT || V_ORD_PEP || V_SSEPARADOR_TXT || V_ORD_DOMICILIO || V_SSEPARADOR_TXT ||
                   V_ORD_DEPART || V_SSEPARADOR_TXT || V_ORD_PROV || V_SSEPARADOR_TXT || V_ORD_DIST || V_SSEPARADOR_TXT || V_SSEPARADOR_TXT || V_ORD_TELEFONO || V_SSEPARADOR_TXT || V_BEN_RELACION ||
                   V_SSEPARADOR_TXT || V_BEN_CONDICION || V_SSEPARADOR_TXT || V_BEN_TIP_PER || V_SSEPARADOR_TXT || V_BEN_TIP_DOC || V_SSEPARADOR_TXT || V_BEN_NUM_DOC || V_SSEPARADOR_TXT ||
                   V_BEN_NUM_RUC || V_SSEPARADOR_TXT || V_BEN_APEPAT || V_SSEPARADOR_TXT || V_BEN_APEMAT || V_SSEPARADOR_TXT || V_BEN_NOMBRES || V_SSEPARADOR_TXT || V_BEN_OCUPACION ||
                   V_SSEPARADOR_TXT || V_BEN_PAIS || V_SSEPARADOR_TXT || V_BEN_CARGO || V_SSEPARADOR_TXT || V_BEN_PEP || V_SSEPARADOR_TXT || V_SSEPARADOR_TXT || V_BEN_DOMICILIO || V_SSEPARADOR_TXT ||
                   V_SSEPARADOR_TXT || V_BEN_DEPART || V_SSEPARADOR_TXT || V_BEN_PROV || V_SSEPARADOR_TXT || V_BEN_DIST || V_SSEPARADOR_TXT || V_BEN_TELEFONO || V_SSEPARADOR_TXT || V_DAT_TIPFON ||
                   V_SSEPARADOR_TXT || V_DAT_TIPOPE || V_SSEPARADOR_TXT || V_DAT_DESOPE || V_SSEPARADOR_TXT || V_DAT_ORIFON || V_SSEPARADOR_TXT || V_DAT_MONOPE || V_SSEPARADOR_TXT || V_DAT_MONOPE_A ||
                   V_SSEPARADOR_TXT || V_DAT_MTOOPE || V_SSEPARADOR_TXT || V_DAT_MTOOPEA || V_SSEPARADOR_TXT || V_DAT_COD_ENT_INVO || V_SSEPARADOR_TXT || V_DAT_COD_TIP_CTAO || V_SSEPARADOR_TXT ||
                   V_DAT_COD_CTAO || V_SSEPARADOR_TXT || V_DAT_ENT_FNC_EXTO || V_SSEPARADOR_TXT || V_DAT_COD_ENT_INVB || V_SSEPARADOR_TXT || V_DAT_COD_TIP_CTAB || V_SSEPARADOR_TXT || V_DAT_COD_CTAB ||
                   V_SSEPARADOR_TXT || V_DAT_ENT_FNC_EXTB || V_SSEPARADOR_TXT || V_DAT_ALCANCE || V_SSEPARADOR_TXT || V_DAT_COD_PAISO || V_SSEPARADOR_TXT || V_DAT_COD_PAISD || V_SSEPARADOR_TXT ||
                   V_DAT_INTOPE || V_SSEPARADOR_TXT || V_DAT_FORMA || V_SSEPARADOR_TXT || V_DAT_INFORM || V_SSEPARADOR_TXT || V_ORIGEN;

          UTL_FILE.PUT_LINE(FILE_OUTPUT, WTEXT);
        END LOOP;

      ELSIF P_EST_REPORTES = 'V' THEN

        VIDA_MAYORES(P_OPE, P_TC, P_MONTO, P_FECINI, P_FECFIN, V_C_TABLE);

        WTEXT := '';
        SETEAR_VARIABLES;

        LOOP
          FETCH V_C_TABLE
            INTO V_FILA,
                 V_OFICINA,
                 V_OPERACION,
                 V_INTERNO,
                 V_MODALIDAD,
                 V_OPE_UBIGEO,
                 V_OPE_FECHA,
                 V_OPE_HORA,
                 V_EJE_RELACION,
                 V_EJE_CONDICION,
                 V_EJE_TIPPER,
                 V_EJE_TIPDOC,
                 V_EJE_NUMDOC,
                 V_EJE_NUMRUC,
                 V_EJE_APEPAT,
                 V_EJE_APEMAT,
                 V_EJE_NOMBRES,
                 V_EJE_OCUPACION,
                 V_EJE_PAIS,
                 V_EJE_CARGO,
                 V_EJE_PEP,
                 V_EJE_DOMICILIO,
                 V_EJE_DEPART,
                 V_EJE_PROV,
                 V_EJE_DIST,
                 V_EJE_TELEFONO,
                 V_ORD_RELACION,
                 V_ORD_CONDICION,
                 V_ORD_TIPPER,
                 V_ORD_TIPDOC,
                 V_ORD_NUMDOC,
                 V_ORD_NUMRUC,
                 V_ORD_APEPAT,
                 V_ORD_APEMAT,
                 V_ORD_NOMBRES,
                 V_ORD_OCUPACION,
                 V_ORD_PAIS,
                 V_ORD_CARGO,
                 V_ORD_PEP,
                 V_ORD_DOMICILIO,
                 V_ORD_DEPART,
                 V_ORD_PROV,
                 V_ORD_DIST,
                 V_ORD_TELEFONO,
                 V_BEN_RELACION,
                 V_BEN_CONDICION,
                 V_BEN_TIP_PER,
                 V_BEN_TIP_DOC,
                 V_BEN_NUM_DOC,
                 V_BEN_NUM_RUC,
                 V_BEN_APEPAT,
                 V_BEN_APEMAT,
                 V_BEN_NOMBRES,
                 V_BEN_OCUPACION,
                 V_BEN_PAIS,
                 V_BEN_CARGO,
                 V_BEN_PEP,
                 V_BEN_DOMICILIO,
                 V_BEN_DEPART,
                 V_BEN_PROV,
                 V_BEN_DIST,
                 V_BEN_TELEFONO,
                 V_DAT_TIPFON,
                 V_DAT_TIPOPE,
                 V_DAT_DESOPE,
                 V_DAT_ORIFON,
                 V_DAT_MONOPE,
                 V_DAT_MONOPE_A,
                 V_DAT_MTOOPE,
                 V_DAT_MTOOPEA,
                 V_DAT_COD_ENT_INVO,
                 V_DAT_COD_TIP_CTAO,
                 V_DAT_COD_CTAO,
                 V_DAT_ENT_FNC_EXTO,
                 V_DAT_COD_ENT_INVB,
                 V_DAT_COD_TIP_CTAB,
                 V_DAT_COD_CTAB,
                 V_DAT_ENT_FNC_EXTB,
                 V_DAT_ALCANCE,
                 V_DAT_COD_PAISO,
                 V_DAT_COD_PAISD,
                 V_DAT_INTOPE,
                 V_DAT_FORMA,
                 V_DAT_INFORM,
                 V_ORIGEN;

          EXIT WHEN V_C_TABLE%NOTFOUND;

          --CUERPO DEL ARCHIVO
          WTEXT := V_FILA || V_SSEPARADOR_TXT || V_OFICINA || V_SSEPARADOR_TXT || V_OPERACION || V_SSEPARADOR_TXT || V_INTERNO || V_SSEPARADOR_TXT || V_MODALIDAD || V_SSEPARADOR_TXT || V_OPE_UBIGEO ||
                   V_SSEPARADOR_TXT || V_OPE_FECHA || V_SSEPARADOR_TXT || V_OPE_HORA || V_SSEPARADOR_TXT || V_EJE_RELACION || V_SSEPARADOR_TXT || V_EJE_CONDICION || V_SSEPARADOR_TXT || V_EJE_TIPPER ||
                   V_SSEPARADOR_TXT || V_EJE_TIPDOC || V_SSEPARADOR_TXT || V_EJE_NUMDOC || V_SSEPARADOR_TXT || V_EJE_NUMRUC || V_SSEPARADOR_TXT || V_EJE_APEPAT || V_SSEPARADOR_TXT || V_EJE_APEMAT ||
                   V_SSEPARADOR_TXT || V_EJE_NOMBRES || V_SSEPARADOR_TXT || V_EJE_OCUPACION || V_SSEPARADOR_TXT || V_EJE_PAIS || V_SSEPARADOR_TXT || V_EJE_CARGO || V_SSEPARADOR_TXT || V_EJE_PEP ||
                   V_SSEPARADOR_TXT || V_EJE_DOMICILIO || V_SSEPARADOR_TXT || V_EJE_DEPART || V_SSEPARADOR_TXT || V_EJE_PROV || V_SSEPARADOR_TXT || V_EJE_DIST || V_SSEPARADOR_TXT || V_EJE_TELEFONO ||
                   V_SSEPARADOR_TXT || V_ORD_RELACION || V_SSEPARADOR_TXT || V_ORD_CONDICION || V_SSEPARADOR_TXT || V_ORD_TIPPER || V_SSEPARADOR_TXT || V_ORD_TIPDOC || V_SSEPARADOR_TXT ||
                   V_ORD_NUMDOC || V_SSEPARADOR_TXT || V_ORD_NUMRUC || V_SSEPARADOR_TXT || V_ORD_APEPAT || V_SSEPARADOR_TXT || V_ORD_APEMAT || V_SSEPARADOR_TXT || V_ORD_NOMBRES || V_SSEPARADOR_TXT ||
                   V_ORD_OCUPACION || V_SSEPARADOR_TXT || V_ORD_PAIS || V_SSEPARADOR_TXT || V_ORD_CARGO || V_SSEPARADOR_TXT || V_ORD_PEP || V_SSEPARADOR_TXT || V_ORD_DIRECCION || V_SSEPARADOR_TXT ||
                   V_ORD_DOMICILIO || V_SSEPARADOR_TXT || V_UBIGEO || V_SSEPARADOR_TXT || V_ORD_DEPART || V_SSEPARADOR_TXT || V_ORD_PROV || V_SSEPARADOR_TXT || V_ORD_DIST || V_SSEPARADOR_TXT ||
                   V_ORD_TELEFONO || V_SSEPARADOR_TXT || V_BEN_RELACION || V_SSEPARADOR_TXT || V_BEN_CONDICION || V_SSEPARADOR_TXT || V_BEN_TIP_PER || V_SSEPARADOR_TXT || V_BEN_TIP_DOC ||
                   V_SSEPARADOR_TXT || V_BEN_NUM_DOC || V_SSEPARADOR_TXT || V_BEN_NUM_RUC || V_SSEPARADOR_TXT || V_BEN_APEPAT || V_SSEPARADOR_TXT || V_BEN_APEMAT || V_SSEPARADOR_TXT || V_BEN_NOMBRES ||
                   V_SSEPARADOR_TXT || V_BEN_OCUPACION || V_SSEPARADOR_TXT || V_BEN_PAIS || V_SSEPARADOR_TXT || V_BEN_CARGO || V_SSEPARADOR_TXT || V_BEN_PEP || V_SSEPARADOR_TXT || V_SSEPARADOR_TXT ||
                   V_BEN_DOMICILIO || V_SSEPARADOR_TXT || V_SSEPARADOR_TXT || V_BEN_DEPART || V_SSEPARADOR_TXT || V_BEN_PROV || V_SSEPARADOR_TXT || V_BEN_DIST || V_SSEPARADOR_TXT || V_BEN_TELEFONO ||
                   V_SSEPARADOR_TXT || V_DAT_TIPFON || V_SSEPARADOR_TXT || V_DAT_TIPOPE || V_SSEPARADOR_TXT || V_DAT_DESOPE || V_SSEPARADOR_TXT || V_DAT_ORIFON || V_SSEPARADOR_TXT || V_DAT_MONOPE ||
                   V_SSEPARADOR_TXT || V_DAT_MONOPE_A || V_SSEPARADOR_TXT || V_DAT_MTOOPE || V_SSEPARADOR_TXT || V_DAT_MTOOPEA || V_SSEPARADOR_TXT || V_DAT_COD_ENT_INVO || V_SSEPARADOR_TXT ||
                   V_DAT_COD_TIP_CTAO || V_SSEPARADOR_TXT || V_DAT_COD_CTAO || V_SSEPARADOR_TXT || V_DAT_ENT_FNC_EXTO || V_SSEPARADOR_TXT || V_DAT_COD_ENT_INVB || V_SSEPARADOR_TXT ||
                   V_DAT_COD_TIP_CTAB || V_SSEPARADOR_TXT || V_DAT_COD_CTAB || V_SSEPARADOR_TXT || V_DAT_ENT_FNC_EXTB || V_SSEPARADOR_TXT || V_DAT_ALCANCE || V_SSEPARADOR_TXT || V_DAT_COD_PAISO ||
                   V_SSEPARADOR_TXT || V_DAT_COD_PAISD || V_SSEPARADOR_TXT || V_DAT_INTOPE || V_SSEPARADOR_TXT || V_DAT_FORMA || V_SSEPARADOR_TXT || V_DAT_INFORM || V_SSEPARADOR_TXT || V_ORIGEN;

          UTL_FILE.PUT_LINE(FILE_OUTPUT, WTEXT);

        END LOOP;

      ELSIF P_EST_REPORTES = 'R' THEN

        RENTAS_MAYORES(P_FECINI, P_FECFIN, P_MONTO, P_TC, V_C_TABLE);

        WTEXT := '';
        SETEAR_VARIABLES;

        LOOP
          FETCH V_C_TABLE
            INTO V_FILA,
                 V_OPERACION,
                 V_OFICINA,
                 V_INTERNO,
                 V_MODALIDAD,
                 V_OPE_UBIGEO,
                 V_OPE_FECHA,
                 V_OPE_HORA,
                 V_EJE_RELACION,
                 V_EJE_CONDICION,
                 V_EJE_TIPPER,
                 V_EJE_TIPDOC,
                 V_EJE_NUMDOC,
                 V_EJE_NUMRUC,
                 V_EJE_APEPAT,
                 V_EJE_APEMAT,
                 V_EJE_NOMBRES,
                 V_EJE_OCUPACION,
                 V_EJE_PAIS,
                 V_EJE_CARGO,
                 V_EJE_PEP,
                 V_EJE_DOMICILIO,
                 V_EJE_DEPART,
                 V_EJE_PROV,
                 V_EJE_DIST,
                 V_EJE_TELEFONO,
                 V_ORD_RELACION,
                 V_ORD_CONDICION,
                 V_ORD_TIPPER,
                 V_ORD_TIPDOC,
                 V_ORD_NUMDOC,
                 V_ORD_NUMRUC,
                 V_ORD_APEPAT,
                 V_ORD_APEMAT,
                 V_ORD_NOMBRES,
                 V_ORD_OCUPACION,
                 V_ORD_PAIS,
                 V_ORD_CARGO,
                 V_ORD_PEP,
                 V_ORD_DOMICILIO,
                 V_ORD_DEPART,
                 V_ORD_PROV,
                 V_ORD_DIST,
                 V_ORD_TELEFONO,
                 V_BEN_RELACION,
                 V_BEN_CONDICION,
                 V_BEN_TIPPER,
                 V_BEN_TIPDOC,
                 V_BEN_NUMDOC,
                 V_BEN_NUMRUC,
                 V_BEN_APEPAT,
                 V_BEN_APEMAT,
                 V_BEN_NOMBRES,
                 V_BEN_OCUPACION,
                 V_BEN_PAIS,
                 V_BEN_CARGO,
                 V_BEN_PEP,
                 V_BEN_DOMICILIO,
                 V_BEN_DEPART,
                 V_BEN_PROV,
                 V_BEN_DIST,
                 V_BEN_TELEFONO,
                 V_DAT_TIPFON,
                 V_DAT_TIPOPE,
                 V_DAT_DESOPE,
                 V_DAT_ORIFON,
                 V_DAT_MONOPE,
                 V_DAT_MONOPEA,
                 V_DAT_MTOOPE,
                 V_DAT_MTOOPEA,
                 V_DAT_COD_ENT_INVO,
                 V_DAT_COD_TIP_CTAO,
                 V_DAT_COD_CTAO,
                 V_DAT_ENT_FNC_EXTO,
                 V_DAT_COD_ENT_INVB,
                 V_DAT_COD_TIP_CTAB,
                 V_DAT_COD_CTAB,
                 V_DAT_ENT_FNC_EXTB,
                 V_DAT_ALCANCE,
                 V_DAT_CODPAISO,
                 V_DAT_CODPAISD,
                 V_DAT_INTERMOPE,
                 V_DAT_FORMA,
                 V_DAT_INFORM,
                 V_TIPO,
                 V_ORIGEN;

          EXIT WHEN V_C_TABLE%NOTFOUND;

          WTEXT := V_FILA || V_SSEPARADOR_TXT || V_OPERACION || V_SSEPARADOR_TXT || V_OFICINA || V_SSEPARADOR_TXT || V_INTERNO || V_SSEPARADOR_TXT || V_MODALIDAD || V_SSEPARADOR_TXT || V_OPE_UBIGEO ||
                   V_SSEPARADOR_TXT || V_OPE_FECHA || V_SSEPARADOR_TXT || V_OPE_HORA || V_SSEPARADOR_TXT || V_EJE_RELACION || V_SSEPARADOR_TXT || V_EJE_CONDICION || V_SSEPARADOR_TXT || V_EJE_TIPPER ||
                   V_SSEPARADOR_TXT || V_EJE_TIPDOC || V_SSEPARADOR_TXT || V_EJE_NUMDOC || V_SSEPARADOR_TXT || V_EJE_NUMRUC || V_SSEPARADOR_TXT || V_EJE_APEPAT || V_SSEPARADOR_TXT || V_EJE_APEMAT ||
                   V_SSEPARADOR_TXT || V_EJE_NOMBRES || V_SSEPARADOR_TXT || V_EJE_OCUPACION || V_SSEPARADOR_TXT || V_EJE_PAIS || V_SSEPARADOR_TXT || V_EJE_CARGO || V_SSEPARADOR_TXT || V_EJE_PEP ||
                   V_SSEPARADOR_TXT || V_EJE_DOMICILIO || V_SSEPARADOR_TXT || V_EJE_DEPART || V_SSEPARADOR_TXT || V_EJE_PROV || V_SSEPARADOR_TXT || V_EJE_DIST || V_SSEPARADOR_TXT || V_EJE_TELEFONO ||
                   V_SSEPARADOR_TXT || V_ORD_RELACION || V_SSEPARADOR_TXT || V_ORD_CONDICION || V_SSEPARADOR_TXT || V_ORD_TIPPER || V_SSEPARADOR_TXT || V_ORD_TIPDOC || V_SSEPARADOR_TXT ||
                   V_ORD_NUMDOC || V_SSEPARADOR_TXT || V_ORD_NUMRUC || V_SSEPARADOR_TXT || V_ORD_APEPAT || V_SSEPARADOR_TXT || V_ORD_APEMAT || V_SSEPARADOR_TXT || V_ORD_NOMBRES || V_SSEPARADOR_TXT ||
                   V_ORD_OCUPACION || V_SSEPARADOR_TXT || V_ORD_PAIS || V_SSEPARADOR_TXT || V_ORD_CARGO || V_SSEPARADOR_TXT || V_ORD_PEP || V_SSEPARADOR_TXT || V_ORD_DOMICILIO || V_SSEPARADOR_TXT ||
                   V_ORD_DEPART || V_SSEPARADOR_TXT || V_ORD_PROV || V_SSEPARADOR_TXT || V_ORD_DIST || V_SSEPARADOR_TXT || V_ORD_TELEFONO || V_SSEPARADOR_TXT || V_BEN_RELACION || V_SSEPARADOR_TXT ||
                   V_BEN_CONDICION || V_SSEPARADOR_TXT || V_BEN_TIPPER || V_SSEPARADOR_TXT || V_BEN_TIPDOC || V_SSEPARADOR_TXT || V_BEN_NUMDOC || V_SSEPARADOR_TXT || V_BEN_NUMRUC || V_SSEPARADOR_TXT ||
                   V_BEN_APEPAT || V_SSEPARADOR_TXT || V_BEN_APEMAT || V_SSEPARADOR_TXT || V_BEN_NOMBRES || V_SSEPARADOR_TXT || V_BEN_OCUPACION || V_SSEPARADOR_TXT || V_BEN_PAIS || V_SSEPARADOR_TXT ||
                   V_BEN_CARGO || V_SSEPARADOR_TXT || V_BEN_PEP || V_SSEPARADOR_TXT || V_BEN_DOMICILIO || V_SSEPARADOR_TXT || V_BEN_DEPART || V_SSEPARADOR_TXT || V_BEN_PROV || V_SSEPARADOR_TXT ||
                   V_BEN_DIST || V_SSEPARADOR_TXT || V_BEN_TELEFONO || V_SSEPARADOR_TXT || V_DAT_TIPFON || V_SSEPARADOR_TXT || V_DAT_TIPOPE || V_SSEPARADOR_TXT || V_DAT_DESOPE || V_SSEPARADOR_TXT ||
                   V_DAT_ORIFON || V_SSEPARADOR_TXT || V_DAT_MONOPE || V_SSEPARADOR_TXT || V_DAT_MONOPEA || V_SSEPARADOR_TXT || V_DAT_MTOOPE || V_SSEPARADOR_TXT || V_DAT_MTOOPEA || V_SSEPARADOR_TXT ||
                   V_DAT_COD_ENT_INVO || V_SSEPARADOR_TXT || V_DAT_COD_TIP_CTAO || V_SSEPARADOR_TXT || V_DAT_COD_CTAO || V_SSEPARADOR_TXT || V_DAT_ENT_FNC_EXTO || V_SSEPARADOR_TXT ||
                   V_DAT_COD_ENT_INVB || V_SSEPARADOR_TXT || V_DAT_COD_TIP_CTAB || V_SSEPARADOR_TXT || V_DAT_COD_CTAB || V_SSEPARADOR_TXT || V_DAT_ENT_FNC_EXTB || V_SSEPARADOR_TXT || V_DAT_ALCANCE ||
                   V_SSEPARADOR_TXT || V_DAT_CODPAISO || V_SSEPARADOR_TXT || V_DAT_CODPAISD || V_SSEPARADOR_TXT || V_DAT_INTERMOPE || V_SSEPARADOR_TXT || V_DAT_FORMA || V_SSEPARADOR_TXT ||
                   V_DAT_INFORM || V_SSEPARADOR_TXT || V_TIPO || V_SSEPARADOR_TXT || V_ORIGEN;

          UTL_FILE.PUT_LINE(FILE_OUTPUT, WTEXT);
        END LOOP;

      ELSIF P_EST_REPORTES = 'SV' THEN

        SINIESTROS_MAYORES(P_OPE, P_TC, P_MONTO, P_FECINI, P_FECFIN, V_C_TABLE);

        WTEXT := '';
        SETEAR_VARIABLES;

        LOOP
          FETCH V_C_TABLE

            INTO V_FILA,
                 V_OFICINA,
                 V_OPERACION,
                 V_INTERNO,
                 V_MODALIDAD,
                 V_OPE_UBIGEO,
                 V_OPE_FECHA,
                 V_OPE_HORA,
                 V_EJE_RELACION,
                 V_EJE_CONDICION,
                 V_EJE_TIPPER,
                 V_EJE_TIPDOC,
                 V_EJE_NUMDOC,
                 V_EJE_NUMRUC,
                 V_EJE_APEPAT,
                 V_EJE_APEMAT,
                 V_EJE_NOMBRES,
                 V_EJE_OCUPACION,
                 V_EJE_PAIS,
                 V_EJE_CARGO,
                 V_EJE_PEP,
                 V_EJE_DOMICILIO,
                 V_EJE_DEPART,
                 V_EJE_PROV,
                 V_EJE_DIST,
                 V_EJE_TELEFONO,
                 V_ORD_RELACION,
                 V_ORD_CONDICION,
                 V_ORD_TIPPER,
                 V_ORD_TIPDOC,
                 V_ORD_NUMDOC,
                 V_ORD_NUMRUC,
                 V_ORD_APEPAT,
                 V_ORD_APEMAT,
                 V_ORD_NOMBRES,
                 V_ORD_OCUPACION,
                 V_ORD_PAIS,
                 V_ORD_CARGO,
                 V_ORD_PEP,
                 V_ORD_DOMICILIO,
                 V_ORD_DEPART,
                 V_ORD_PROV,
                 V_ORD_DIST,

                 V_ORD_TELEFONO,
                 V_BEN_RELACION,
                 V_BEN_CONDICION,
                 V_BEN_TIP_PER,
                 V_BEN_TIP_DOC,
                 V_BEN_NUM_DOC,
                 V_BEN_NUM_RUC,
                 V_BEN_APEPAT,
                 V_BEN_APEMAT,
                 V_BEN_NOMBRES,
                 V_BEN_OCUPACION,
                 V_BEN_PAIS,
                 V_BEN_CARGO,
                 V_BEN_PEP,

                 V_BEN_DOMICILIO,

                 V_BEN_DEPART,
                 V_BEN_PROV,
                 V_BEN_DIST,
                 V_BEN_TELEFONO,
                 V_DAT_TIPFON,
                 V_DAT_TIPOPE,
                 V_DAT_DESOPE,
                 V_DAT_ORIFON,
                 V_DAT_MONOPE,
                 V_DAT_MONOPE_A,
                 V_DAT_MTOOPE,
                 V_DAT_MTOOPEA,
                 V_DAT_COD_ENT_INVO,
                 V_DAT_COD_TIP_CTAO,
                 V_DAT_COD_CTAO,
                 V_DAT_ENT_FNC_EXTO,
                 V_DAT_COD_ENT_INVB,
                 V_DAT_COD_TIP_CTAB,
                 V_DAT_COD_CTAB,
                 V_DAT_ENT_FNC_EXTB,
                 V_DAT_ALCANCE,
                 V_DAT_COD_PAISO,
                 V_DAT_COD_PAISD,
                 V_DAT_INTOPE,
                 V_DAT_FORMA,
                 V_DAT_INFORM,
                 V_ORIGEN;

          EXIT WHEN V_C_TABLE%NOTFOUND;

          --CUERPO DEL ARCHIVO
          WTEXT := V_FILA || V_SSEPARADOR_TXT || V_OFICINA || V_SSEPARADOR_TXT || V_OPERACION || V_SSEPARADOR_TXT || V_INTERNO || V_SSEPARADOR_TXT || V_MODALIDAD || V_SSEPARADOR_TXT || V_OPE_UBIGEO ||
                   V_SSEPARADOR_TXT || V_OPE_FECHA || V_SSEPARADOR_TXT || V_OPE_HORA || V_SSEPARADOR_TXT || V_EJE_RELACION || V_SSEPARADOR_TXT || V_EJE_CONDICION || V_SSEPARADOR_TXT || V_EJE_TIPPER ||
                   V_SSEPARADOR_TXT || V_EJE_TIPDOC || V_SSEPARADOR_TXT || V_EJE_NUMDOC || V_SSEPARADOR_TXT || V_EJE_NUMRUC || V_SSEPARADOR_TXT || V_EJE_APEPAT || V_SSEPARADOR_TXT || V_EJE_APEMAT ||
                   V_SSEPARADOR_TXT || V_EJE_NOMBRES || V_SSEPARADOR_TXT || V_EJE_OCUPACION || V_SSEPARADOR_TXT || V_EJE_PAIS || V_SSEPARADOR_TXT || V_EJE_CARGO || V_SSEPARADOR_TXT || V_EJE_PEP ||
                   V_SSEPARADOR_TXT || V_EJE_DOMICILIO || V_SSEPARADOR_TXT || V_EJE_DEPART || V_SSEPARADOR_TXT || V_EJE_PROV || V_SSEPARADOR_TXT || V_EJE_DIST || V_SSEPARADOR_TXT || V_EJE_TELEFONO ||
                   V_SSEPARADOR_TXT || V_ORD_RELACION || V_SSEPARADOR_TXT || V_ORD_CONDICION || V_SSEPARADOR_TXT || V_ORD_TIPPER || V_SSEPARADOR_TXT || V_ORD_TIPDOC || V_SSEPARADOR_TXT ||
                   V_ORD_NUMDOC || V_SSEPARADOR_TXT || V_ORD_NUMRUC || V_SSEPARADOR_TXT || V_ORD_APEPAT || V_SSEPARADOR_TXT || V_ORD_APEMAT || V_SSEPARADOR_TXT || V_ORD_NOMBRES || V_SSEPARADOR_TXT ||
                   V_ORD_OCUPACION || V_SSEPARADOR_TXT || V_ORD_PAIS || V_SSEPARADOR_TXT || V_ORD_CARGO || V_SSEPARADOR_TXT || V_ORD_PEP || V_SSEPARADOR_TXT || V_ORD_DOMICILIO || V_SSEPARADOR_TXT ||
                   V_ORD_DEPART || V_SSEPARADOR_TXT || V_ORD_PROV || V_SSEPARADOR_TXT || V_ORD_DIST || V_SSEPARADOR_TXT || V_SSEPARADOR_TXT || V_ORD_TELEFONO || V_SSEPARADOR_TXT || V_BEN_RELACION ||
                   V_SSEPARADOR_TXT || V_BEN_CONDICION || V_SSEPARADOR_TXT || V_BEN_TIP_PER || V_SSEPARADOR_TXT || V_BEN_TIP_DOC || V_SSEPARADOR_TXT || V_BEN_NUM_DOC || V_SSEPARADOR_TXT ||
                   V_BEN_NUM_RUC || V_SSEPARADOR_TXT || V_BEN_APEPAT || V_SSEPARADOR_TXT || V_BEN_APEMAT || V_SSEPARADOR_TXT || V_BEN_NOMBRES || V_SSEPARADOR_TXT || V_BEN_OCUPACION ||
                   V_SSEPARADOR_TXT || V_BEN_PAIS || V_SSEPARADOR_TXT || V_BEN_CARGO || V_SSEPARADOR_TXT || V_BEN_PEP || V_SSEPARADOR_TXT || V_SSEPARADOR_TXT || V_BEN_DOMICILIO || V_SSEPARADOR_TXT ||
                   V_SSEPARADOR_TXT || V_BEN_DEPART || V_SSEPARADOR_TXT || V_BEN_PROV || V_SSEPARADOR_TXT || V_BEN_DIST || V_SSEPARADOR_TXT || V_BEN_TELEFONO || V_SSEPARADOR_TXT || V_DAT_TIPFON ||
                   V_SSEPARADOR_TXT || V_DAT_TIPOPE || V_SSEPARADOR_TXT || V_DAT_DESOPE || V_SSEPARADOR_TXT || V_DAT_ORIFON || V_SSEPARADOR_TXT || V_DAT_MONOPE || V_SSEPARADOR_TXT || V_DAT_MONOPE_A ||
                   V_SSEPARADOR_TXT || V_DAT_MTOOPE || V_SSEPARADOR_TXT || V_DAT_MTOOPEA || V_SSEPARADOR_TXT || V_DAT_COD_ENT_INVO || V_SSEPARADOR_TXT || V_DAT_COD_TIP_CTAO || V_SSEPARADOR_TXT ||
                   V_DAT_COD_CTAO || V_SSEPARADOR_TXT || V_DAT_ENT_FNC_EXTO || V_SSEPARADOR_TXT || V_DAT_COD_ENT_INVB || V_SSEPARADOR_TXT || V_DAT_COD_TIP_CTAB || V_SSEPARADOR_TXT || V_DAT_COD_CTAB ||
                   V_SSEPARADOR_TXT || V_DAT_ENT_FNC_EXTB || V_SSEPARADOR_TXT || V_DAT_ALCANCE || V_SSEPARADOR_TXT || V_DAT_COD_PAISO || V_SSEPARADOR_TXT || V_DAT_COD_PAISD || V_SSEPARADOR_TXT ||
                   V_DAT_INTOPE || V_SSEPARADOR_TXT || V_DAT_FORMA || V_SSEPARADOR_TXT || V_DAT_INFORM || V_SSEPARADOR_TXT || V_ORIGEN;

          UTL_FILE.PUT_LINE(FILE_OUTPUT, WTEXT);

        END LOOP;

        VIDA_MAYORES(P_OPE, P_TC, P_MONTO, P_FECINI, P_FECFIN, V_C_TABLE);

        WTEXT := '';
        SETEAR_VARIABLES;

        LOOP
          FETCH V_C_TABLE
            INTO V_FILA,
                 V_OFICINA,
                 V_OPERACION,
                 V_INTERNO,
                 V_MODALIDAD,
                 V_OPE_UBIGEO,
                 V_OPE_FECHA,
                 V_OPE_HORA,
                 V_EJE_RELACION,
                 V_EJE_CONDICION,
                 V_EJE_TIPPER,
                 V_EJE_TIPDOC,
                 V_EJE_NUMDOC,
                 V_EJE_NUMRUC,
                 V_EJE_APEPAT,
                 V_EJE_APEMAT,
                 V_EJE_NOMBRES,
                 V_EJE_OCUPACION,
                 V_EJE_PAIS,
                 V_EJE_CARGO,
                 V_EJE_PEP,
                 V_EJE_DOMICILIO,
                 V_EJE_DEPART,
                 V_EJE_PROV,
                 V_EJE_DIST,
                 V_EJE_TELEFONO,
                 V_ORD_RELACION,
                 V_ORD_CONDICION,
                 V_ORD_TIPPER,
                 V_ORD_TIPDOC,
                 V_ORD_NUMDOC,
                 V_ORD_NUMRUC,
                 V_ORD_APEPAT,
                 V_ORD_APEMAT,
                 V_ORD_NOMBRES,
                 V_ORD_OCUPACION,
                 V_ORD_PAIS,
                 V_ORD_CARGO,
                 V_ORD_PEP,
                 V_ORD_DOMICILIO,
                 V_ORD_DEPART,
                 V_ORD_PROV,
                 V_ORD_DIST,
                 V_ORD_TELEFONO,
                 V_BEN_RELACION,
                 V_BEN_CONDICION,
                 V_BEN_TIP_PER,
                 V_BEN_TIP_DOC,
                 V_BEN_NUM_DOC,
                 V_BEN_NUM_RUC,
                 V_BEN_APEPAT,
                 V_BEN_APEMAT,
                 V_BEN_NOMBRES,
                 V_BEN_OCUPACION,
                 V_BEN_PAIS,
                 V_BEN_CARGO,
                 V_BEN_PEP,
                 V_BEN_DOMICILIO,
                 V_BEN_DEPART,
                 V_BEN_PROV,
                 V_BEN_DIST,
                 V_BEN_TELEFONO,
                 V_DAT_TIPFON,
                 V_DAT_TIPOPE,
                 V_DAT_DESOPE,
                 V_DAT_ORIFON,
                 V_DAT_MONOPE,
                 V_DAT_MONOPE_A,
                 V_DAT_MTOOPE,
                 V_DAT_MTOOPEA,
                 V_DAT_COD_ENT_INVO,
                 V_DAT_COD_TIP_CTAO,
                 V_DAT_COD_CTAO,
                 V_DAT_ENT_FNC_EXTO,
                 V_DAT_COD_ENT_INVB,
                 V_DAT_COD_TIP_CTAB,
                 V_DAT_COD_CTAB,
                 V_DAT_ENT_FNC_EXTB,
                 V_DAT_ALCANCE,
                 V_DAT_COD_PAISO,
                 V_DAT_COD_PAISD,
                 V_DAT_INTOPE,
                 V_DAT_FORMA,
                 V_DAT_INFORM,
                 V_ORIGEN;

          EXIT WHEN V_C_TABLE%NOTFOUND;

          --CUERPO DEL ARCHIVO
          WTEXT := V_FILA || V_SSEPARADOR_TXT || V_OFICINA || V_SSEPARADOR_TXT || V_OPERACION || V_SSEPARADOR_TXT || V_INTERNO || V_SSEPARADOR_TXT || V_MODALIDAD || V_SSEPARADOR_TXT || V_OPE_UBIGEO ||
                   V_SSEPARADOR_TXT || V_OPE_FECHA || V_SSEPARADOR_TXT || V_OPE_HORA || V_SSEPARADOR_TXT || V_EJE_RELACION || V_SSEPARADOR_TXT || V_EJE_CONDICION || V_SSEPARADOR_TXT || V_EJE_TIPPER ||
                   V_SSEPARADOR_TXT || V_EJE_TIPDOC || V_SSEPARADOR_TXT || V_EJE_NUMDOC || V_SSEPARADOR_TXT || V_EJE_NUMRUC || V_SSEPARADOR_TXT || V_EJE_APEPAT || V_SSEPARADOR_TXT || V_EJE_APEMAT ||
                   V_SSEPARADOR_TXT || V_EJE_NOMBRES || V_SSEPARADOR_TXT || V_EJE_OCUPACION || V_SSEPARADOR_TXT || V_EJE_PAIS || V_SSEPARADOR_TXT || V_EJE_CARGO || V_SSEPARADOR_TXT || V_EJE_PEP ||
                   V_SSEPARADOR_TXT || V_EJE_DOMICILIO || V_SSEPARADOR_TXT || V_EJE_DEPART || V_SSEPARADOR_TXT || V_EJE_PROV || V_SSEPARADOR_TXT || V_EJE_DIST || V_SSEPARADOR_TXT || V_EJE_TELEFONO ||
                   V_SSEPARADOR_TXT || V_ORD_RELACION || V_SSEPARADOR_TXT || V_ORD_CONDICION || V_SSEPARADOR_TXT || V_ORD_TIPPER || V_SSEPARADOR_TXT || V_ORD_TIPDOC || V_SSEPARADOR_TXT ||
                   V_ORD_NUMDOC || V_SSEPARADOR_TXT || V_ORD_NUMRUC || V_SSEPARADOR_TXT || V_ORD_APEPAT || V_SSEPARADOR_TXT || V_ORD_APEMAT || V_SSEPARADOR_TXT || V_ORD_NOMBRES || V_SSEPARADOR_TXT ||
                   V_ORD_OCUPACION || V_SSEPARADOR_TXT || V_ORD_PAIS || V_SSEPARADOR_TXT || V_ORD_CARGO || V_SSEPARADOR_TXT || V_ORD_PEP || V_SSEPARADOR_TXT || V_ORD_DIRECCION || V_SSEPARADOR_TXT ||
                   V_ORD_DOMICILIO || V_SSEPARADOR_TXT || V_UBIGEO || V_SSEPARADOR_TXT || V_ORD_DEPART || V_SSEPARADOR_TXT || V_ORD_PROV || V_SSEPARADOR_TXT || V_ORD_DIST || V_SSEPARADOR_TXT ||
                   V_ORD_TELEFONO || V_SSEPARADOR_TXT || V_BEN_RELACION || V_SSEPARADOR_TXT || V_BEN_CONDICION || V_SSEPARADOR_TXT || V_BEN_TIP_PER || V_SSEPARADOR_TXT || V_BEN_TIP_DOC ||
                   V_SSEPARADOR_TXT || V_BEN_NUM_DOC || V_SSEPARADOR_TXT || V_BEN_NUM_RUC || V_SSEPARADOR_TXT || V_BEN_APEPAT || V_SSEPARADOR_TXT || V_BEN_APEMAT || V_SSEPARADOR_TXT || V_BEN_NOMBRES ||
                   V_SSEPARADOR_TXT || V_BEN_OCUPACION || V_SSEPARADOR_TXT || V_BEN_PAIS || V_SSEPARADOR_TXT || V_BEN_CARGO || V_SSEPARADOR_TXT || V_BEN_PEP || V_SSEPARADOR_TXT || V_SSEPARADOR_TXT ||
                   V_BEN_DOMICILIO || V_SSEPARADOR_TXT || V_SSEPARADOR_TXT || V_BEN_DEPART || V_SSEPARADOR_TXT || V_BEN_PROV || V_SSEPARADOR_TXT || V_BEN_DIST || V_SSEPARADOR_TXT || V_BEN_TELEFONO ||
                   V_SSEPARADOR_TXT || V_DAT_TIPFON || V_SSEPARADOR_TXT || V_DAT_TIPOPE || V_SSEPARADOR_TXT || V_DAT_DESOPE || V_SSEPARADOR_TXT || V_DAT_ORIFON || V_SSEPARADOR_TXT || V_DAT_MONOPE ||
                   V_SSEPARADOR_TXT || V_DAT_MONOPE_A || V_SSEPARADOR_TXT || V_DAT_MTOOPE || V_SSEPARADOR_TXT || V_DAT_MTOOPEA || V_SSEPARADOR_TXT || V_DAT_COD_ENT_INVO || V_SSEPARADOR_TXT ||
                   V_DAT_COD_TIP_CTAO || V_SSEPARADOR_TXT || V_DAT_COD_CTAO || V_SSEPARADOR_TXT || V_DAT_ENT_FNC_EXTO || V_SSEPARADOR_TXT || V_DAT_COD_ENT_INVB || V_SSEPARADOR_TXT ||
                   V_DAT_COD_TIP_CTAB || V_SSEPARADOR_TXT || V_DAT_COD_CTAB || V_SSEPARADOR_TXT || V_DAT_ENT_FNC_EXTB || V_SSEPARADOR_TXT || V_DAT_ALCANCE || V_SSEPARADOR_TXT || V_DAT_COD_PAISO ||
                   V_SSEPARADOR_TXT || V_DAT_COD_PAISD || V_SSEPARADOR_TXT || V_DAT_INTOPE || V_SSEPARADOR_TXT || V_DAT_FORMA || V_SSEPARADOR_TXT || V_DAT_INFORM || V_SSEPARADOR_TXT || V_ORIGEN;

          UTL_FILE.PUT_LINE(FILE_OUTPUT, WTEXT);

        END LOOP;

      ELSIF P_EST_REPORTES = 'SR' THEN

        SINIESTROS_MAYORES(P_OPE, P_TC, P_MONTO, P_FECINI, P_FECFIN, V_C_TABLE);

        WTEXT := '';
        SETEAR_VARIABLES;

        LOOP
          FETCH V_C_TABLE

            INTO V_FILA,
                 V_OFICINA,
                 V_OPERACION,
                 V_INTERNO,
                 V_MODALIDAD,
                 V_OPE_UBIGEO,
                 V_OPE_FECHA,
                 V_OPE_HORA,
                 V_EJE_RELACION,
                 V_EJE_CONDICION,
                 V_EJE_TIPPER,
                 V_EJE_TIPDOC,
                 V_EJE_NUMDOC,
                 V_EJE_NUMRUC,
                 V_EJE_APEPAT,
                 V_EJE_APEMAT,
                 V_EJE_NOMBRES,
                 V_EJE_OCUPACION,
                 V_EJE_PAIS,
                 V_EJE_CARGO,
                 V_EJE_PEP,
                 V_EJE_DOMICILIO,
                 V_EJE_DEPART,
                 V_EJE_PROV,
                 V_EJE_DIST,
                 V_EJE_TELEFONO,
                 V_ORD_RELACION,
                 V_ORD_CONDICION,
                 V_ORD_TIPPER,
                 V_ORD_TIPDOC,
                 V_ORD_NUMDOC,
                 V_ORD_NUMRUC,
                 V_ORD_APEPAT,
                 V_ORD_APEMAT,
                 V_ORD_NOMBRES,
                 V_ORD_OCUPACION,
                 V_ORD_PAIS,
                 V_ORD_CARGO,
                 V_ORD_PEP,
                 V_ORD_DOMICILIO,
                 V_ORD_DEPART,
                 V_ORD_PROV,
                 V_ORD_DIST,

                 V_ORD_TELEFONO,
                 V_BEN_RELACION,
                 V_BEN_CONDICION,
                 V_BEN_TIP_PER,
                 V_BEN_TIP_DOC,
                 V_BEN_NUM_DOC,
                 V_BEN_NUM_RUC,
                 V_BEN_APEPAT,
                 V_BEN_APEMAT,
                 V_BEN_NOMBRES,
                 V_BEN_OCUPACION,
                 V_BEN_PAIS,
                 V_BEN_CARGO,
                 V_BEN_PEP,

                 V_BEN_DOMICILIO,

                 V_BEN_DEPART,
                 V_BEN_PROV,
                 V_BEN_DIST,
                 V_BEN_TELEFONO,
                 V_DAT_TIPFON,
                 V_DAT_TIPOPE,
                 V_DAT_DESOPE,
                 V_DAT_ORIFON,
                 V_DAT_MONOPE,
                 V_DAT_MONOPE_A,
                 V_DAT_MTOOPE,
                 V_DAT_MTOOPEA,
                 V_DAT_COD_ENT_INVO,
                 V_DAT_COD_TIP_CTAO,
                 V_DAT_COD_CTAO,
                 V_DAT_ENT_FNC_EXTO,
                 V_DAT_COD_ENT_INVB,
                 V_DAT_COD_TIP_CTAB,
                 V_DAT_COD_CTAB,
                 V_DAT_ENT_FNC_EXTB,
                 V_DAT_ALCANCE,
                 V_DAT_COD_PAISO,
                 V_DAT_COD_PAISD,
                 V_DAT_INTOPE,
                 V_DAT_FORMA,
                 V_DAT_INFORM,
                 V_ORIGEN;

          EXIT WHEN V_C_TABLE%NOTFOUND;

          --CUERPO DEL ARCHIVO
          WTEXT := V_FILA || V_SSEPARADOR_TXT || V_OFICINA || V_SSEPARADOR_TXT || V_OPERACION || V_SSEPARADOR_TXT || V_INTERNO || V_SSEPARADOR_TXT || V_MODALIDAD || V_SSEPARADOR_TXT || V_OPE_UBIGEO ||
                   V_SSEPARADOR_TXT || V_OPE_FECHA || V_SSEPARADOR_TXT || V_OPE_HORA || V_SSEPARADOR_TXT || V_EJE_RELACION || V_SSEPARADOR_TXT || V_EJE_CONDICION || V_SSEPARADOR_TXT || V_EJE_TIPPER ||
                   V_SSEPARADOR_TXT || V_EJE_TIPDOC || V_SSEPARADOR_TXT || V_EJE_NUMDOC || V_SSEPARADOR_TXT || V_EJE_NUMRUC || V_SSEPARADOR_TXT || V_EJE_APEPAT || V_SSEPARADOR_TXT || V_EJE_APEMAT ||
                   V_SSEPARADOR_TXT || V_EJE_NOMBRES || V_SSEPARADOR_TXT || V_EJE_OCUPACION || V_SSEPARADOR_TXT || V_EJE_PAIS || V_SSEPARADOR_TXT || V_EJE_CARGO || V_SSEPARADOR_TXT || V_EJE_PEP ||
                   V_SSEPARADOR_TXT || V_EJE_DOMICILIO || V_SSEPARADOR_TXT || V_EJE_DEPART || V_SSEPARADOR_TXT || V_EJE_PROV || V_SSEPARADOR_TXT || V_EJE_DIST || V_SSEPARADOR_TXT || V_EJE_TELEFONO ||
                   V_SSEPARADOR_TXT || V_ORD_RELACION || V_SSEPARADOR_TXT || V_ORD_CONDICION || V_SSEPARADOR_TXT || V_ORD_TIPPER || V_SSEPARADOR_TXT || V_ORD_TIPDOC || V_SSEPARADOR_TXT ||
                   V_ORD_NUMDOC || V_SSEPARADOR_TXT || V_ORD_NUMRUC || V_SSEPARADOR_TXT || V_ORD_APEPAT || V_SSEPARADOR_TXT || V_ORD_APEMAT || V_SSEPARADOR_TXT || V_ORD_NOMBRES || V_SSEPARADOR_TXT ||
                   V_ORD_OCUPACION || V_SSEPARADOR_TXT || V_ORD_PAIS || V_SSEPARADOR_TXT || V_ORD_CARGO || V_SSEPARADOR_TXT || V_ORD_PEP || V_SSEPARADOR_TXT || V_ORD_DOMICILIO || V_SSEPARADOR_TXT ||
                   V_ORD_DEPART || V_SSEPARADOR_TXT || V_ORD_PROV || V_SSEPARADOR_TXT || V_ORD_DIST || V_SSEPARADOR_TXT || V_SSEPARADOR_TXT || V_ORD_TELEFONO || V_SSEPARADOR_TXT || V_BEN_RELACION ||
                   V_SSEPARADOR_TXT || V_BEN_CONDICION || V_SSEPARADOR_TXT || V_BEN_TIP_PER || V_SSEPARADOR_TXT || V_BEN_TIP_DOC || V_SSEPARADOR_TXT || V_BEN_NUM_DOC || V_SSEPARADOR_TXT ||
                   V_BEN_NUM_RUC || V_SSEPARADOR_TXT || V_BEN_APEPAT || V_SSEPARADOR_TXT || V_BEN_APEMAT || V_SSEPARADOR_TXT || V_BEN_NOMBRES || V_SSEPARADOR_TXT || V_BEN_OCUPACION ||
                   V_SSEPARADOR_TXT || V_BEN_PAIS || V_SSEPARADOR_TXT || V_BEN_CARGO || V_SSEPARADOR_TXT || V_BEN_PEP || V_SSEPARADOR_TXT || V_SSEPARADOR_TXT || V_BEN_DOMICILIO || V_SSEPARADOR_TXT ||
                   V_SSEPARADOR_TXT || V_BEN_DEPART || V_SSEPARADOR_TXT || V_BEN_PROV || V_SSEPARADOR_TXT || V_BEN_DIST || V_SSEPARADOR_TXT || V_BEN_TELEFONO || V_SSEPARADOR_TXT || V_DAT_TIPFON ||
                   V_SSEPARADOR_TXT || V_DAT_TIPOPE || V_SSEPARADOR_TXT || V_DAT_DESOPE || V_SSEPARADOR_TXT || V_DAT_ORIFON || V_SSEPARADOR_TXT || V_DAT_MONOPE || V_SSEPARADOR_TXT || V_DAT_MONOPE_A ||
                   V_SSEPARADOR_TXT || V_DAT_MTOOPE || V_SSEPARADOR_TXT || V_DAT_MTOOPEA || V_SSEPARADOR_TXT || V_DAT_COD_ENT_INVO || V_SSEPARADOR_TXT || V_DAT_COD_TIP_CTAO || V_SSEPARADOR_TXT ||
                   V_DAT_COD_CTAO || V_SSEPARADOR_TXT || V_DAT_ENT_FNC_EXTO || V_SSEPARADOR_TXT || V_DAT_COD_ENT_INVB || V_SSEPARADOR_TXT || V_DAT_COD_TIP_CTAB || V_SSEPARADOR_TXT || V_DAT_COD_CTAB ||
                   V_SSEPARADOR_TXT || V_DAT_ENT_FNC_EXTB || V_SSEPARADOR_TXT || V_DAT_ALCANCE || V_SSEPARADOR_TXT || V_DAT_COD_PAISO || V_SSEPARADOR_TXT || V_DAT_COD_PAISD || V_SSEPARADOR_TXT ||
                   V_DAT_INTOPE || V_SSEPARADOR_TXT || V_DAT_FORMA || V_SSEPARADOR_TXT || V_DAT_INFORM || V_SSEPARADOR_TXT || V_ORIGEN;

          UTL_FILE.PUT_LINE(FILE_OUTPUT, WTEXT);
        END LOOP;

        RENTAS_MAYORES(P_FECINI, P_FECFIN, P_MONTO, P_TC, V_C_TABLE);

        WTEXT := '';
        SETEAR_VARIABLES;

        LOOP
          FETCH V_C_TABLE
            INTO V_FILA,
                 V_OPERACION,
                 V_OFICINA,
                 V_INTERNO,
                 V_MODALIDAD,
                 V_OPE_UBIGEO,
                 V_OPE_FECHA,
                 V_OPE_HORA,
                 V_EJE_RELACION,
                 V_EJE_CONDICION,
                 V_EJE_TIPPER,
                 V_EJE_TIPDOC,
                 V_EJE_NUMDOC,
                 V_EJE_NUMRUC,
                 V_EJE_APEPAT,
                 V_EJE_APEMAT,
                 V_EJE_NOMBRES,
                 V_EJE_OCUPACION,
                 V_EJE_PAIS,
                 V_EJE_CARGO,
                 V_EJE_PEP,
                 V_EJE_DOMICILIO,
                 V_EJE_DEPART,
                 V_EJE_PROV,
                 V_EJE_DIST,
                 V_EJE_TELEFONO,
                 V_ORD_RELACION,
                 V_ORD_CONDICION,
                 V_ORD_TIPPER,
                 V_ORD_TIPDOC,
                 V_ORD_NUMDOC,
                 V_ORD_NUMRUC,
                 V_ORD_APEPAT,
                 V_ORD_APEMAT,
                 V_ORD_NOMBRES,
                 V_ORD_OCUPACION,
                 V_ORD_PAIS,
                 V_ORD_CARGO,
                 V_ORD_PEP,
                 V_ORD_DOMICILIO,
                 V_ORD_DEPART,
                 V_ORD_PROV,
                 V_ORD_DIST,
                 V_ORD_TELEFONO,
                 V_BEN_RELACION,
                 V_BEN_CONDICION,
                 V_BEN_TIPPER,
                 V_BEN_TIPDOC,
                 V_BEN_NUMDOC,
                 V_BEN_NUMRUC,
                 V_BEN_APEPAT,
                 V_BEN_APEMAT,
                 V_BEN_NOMBRES,
                 V_BEN_OCUPACION,
                 V_BEN_PAIS,
                 V_BEN_CARGO,
                 V_BEN_PEP,
                 V_BEN_DOMICILIO,
                 V_BEN_DEPART,
                 V_BEN_PROV,
                 V_BEN_DIST,
                 V_BEN_TELEFONO,
                 V_DAT_TIPFON,
                 V_DAT_TIPOPE,
                 V_DAT_DESOPE,
                 V_DAT_ORIFON,
                 V_DAT_MONOPE,
                 V_DAT_MONOPEA,
                 V_DAT_MTOOPE,
                 V_DAT_MTOOPEA,
                 V_DAT_COD_ENT_INVO,
                 V_DAT_COD_TIP_CTAO,
                 V_DAT_COD_CTAO,
                 V_DAT_ENT_FNC_EXTO,
                 V_DAT_COD_ENT_INVB,
                 V_DAT_COD_TIP_CTAB,
                 V_DAT_COD_CTAB,
                 V_DAT_ENT_FNC_EXTB,
                 V_DAT_ALCANCE,
                 V_DAT_CODPAISO,
                 V_DAT_CODPAISD,
                 V_DAT_INTERMOPE,
                 V_DAT_FORMA,
                 V_DAT_INFORM,
                 V_TIPO,
                 V_ORIGEN;

          EXIT WHEN V_C_TABLE%NOTFOUND;

          WTEXT := V_FILA || V_SSEPARADOR_TXT || V_OPERACION || V_SSEPARADOR_TXT || V_OFICINA || V_SSEPARADOR_TXT || V_INTERNO || V_SSEPARADOR_TXT || V_MODALIDAD || V_SSEPARADOR_TXT || V_OPE_UBIGEO ||
                   V_SSEPARADOR_TXT || V_OPE_FECHA || V_SSEPARADOR_TXT || V_OPE_HORA || V_SSEPARADOR_TXT || V_EJE_RELACION || V_SSEPARADOR_TXT || V_EJE_CONDICION || V_SSEPARADOR_TXT || V_EJE_TIPPER ||
                   V_SSEPARADOR_TXT || V_EJE_TIPDOC || V_SSEPARADOR_TXT || V_EJE_NUMDOC || V_SSEPARADOR_TXT || V_EJE_NUMRUC || V_SSEPARADOR_TXT || V_EJE_APEPAT || V_SSEPARADOR_TXT || V_EJE_APEMAT ||
                   V_SSEPARADOR_TXT || V_EJE_NOMBRES || V_SSEPARADOR_TXT || V_EJE_OCUPACION || V_SSEPARADOR_TXT || V_EJE_PAIS || V_SSEPARADOR_TXT || V_EJE_CARGO || V_SSEPARADOR_TXT || V_EJE_PEP ||
                   V_SSEPARADOR_TXT || V_EJE_DOMICILIO || V_SSEPARADOR_TXT || V_EJE_DEPART || V_SSEPARADOR_TXT || V_EJE_PROV || V_SSEPARADOR_TXT || V_EJE_DIST || V_SSEPARADOR_TXT || V_EJE_TELEFONO ||
                   V_SSEPARADOR_TXT || V_ORD_RELACION || V_SSEPARADOR_TXT || V_ORD_CONDICION || V_SSEPARADOR_TXT || V_ORD_TIPPER || V_SSEPARADOR_TXT || V_ORD_TIPDOC || V_SSEPARADOR_TXT ||
                   V_ORD_NUMDOC || V_SSEPARADOR_TXT || V_ORD_NUMRUC || V_SSEPARADOR_TXT || V_ORD_APEPAT || V_SSEPARADOR_TXT || V_ORD_APEMAT || V_SSEPARADOR_TXT || V_ORD_NOMBRES || V_SSEPARADOR_TXT ||
                   V_ORD_OCUPACION || V_SSEPARADOR_TXT || V_ORD_PAIS || V_SSEPARADOR_TXT || V_ORD_CARGO || V_SSEPARADOR_TXT || V_ORD_PEP || V_SSEPARADOR_TXT || V_ORD_DOMICILIO || V_SSEPARADOR_TXT ||
                   V_ORD_DEPART || V_SSEPARADOR_TXT || V_ORD_PROV || V_SSEPARADOR_TXT || V_ORD_DIST || V_SSEPARADOR_TXT || V_ORD_TELEFONO || V_SSEPARADOR_TXT || V_BEN_RELACION || V_SSEPARADOR_TXT ||
                   V_BEN_CONDICION || V_SSEPARADOR_TXT || V_BEN_TIPPER || V_SSEPARADOR_TXT || V_BEN_TIPDOC || V_SSEPARADOR_TXT || V_BEN_NUMDOC || V_SSEPARADOR_TXT || V_BEN_NUMRUC || V_SSEPARADOR_TXT ||
                   V_BEN_APEPAT || V_SSEPARADOR_TXT || V_BEN_APEMAT || V_SSEPARADOR_TXT || V_BEN_NOMBRES || V_SSEPARADOR_TXT || V_BEN_OCUPACION || V_SSEPARADOR_TXT || V_BEN_PAIS || V_SSEPARADOR_TXT ||
                   V_BEN_CARGO || V_SSEPARADOR_TXT || V_BEN_PEP || V_SSEPARADOR_TXT || V_BEN_DOMICILIO || V_SSEPARADOR_TXT || V_BEN_DEPART || V_SSEPARADOR_TXT || V_BEN_PROV || V_SSEPARADOR_TXT ||
                   V_BEN_DIST || V_SSEPARADOR_TXT || V_BEN_TELEFONO || V_SSEPARADOR_TXT || V_DAT_TIPFON || V_SSEPARADOR_TXT || V_DAT_TIPOPE || V_SSEPARADOR_TXT || V_DAT_DESOPE || V_SSEPARADOR_TXT ||
                   V_DAT_ORIFON || V_SSEPARADOR_TXT || V_DAT_MONOPE || V_SSEPARADOR_TXT || V_DAT_MONOPEA || V_SSEPARADOR_TXT || V_DAT_MTOOPE || V_SSEPARADOR_TXT || V_DAT_MTOOPEA || V_SSEPARADOR_TXT ||
                   V_DAT_COD_ENT_INVO || V_SSEPARADOR_TXT || V_DAT_COD_TIP_CTAO || V_SSEPARADOR_TXT || V_DAT_COD_CTAO || V_SSEPARADOR_TXT || V_DAT_ENT_FNC_EXTO || V_SSEPARADOR_TXT ||
                   V_DAT_COD_ENT_INVB || V_SSEPARADOR_TXT || V_DAT_COD_TIP_CTAB || V_SSEPARADOR_TXT || V_DAT_COD_CTAB || V_SSEPARADOR_TXT || V_DAT_ENT_FNC_EXTB || V_SSEPARADOR_TXT || V_DAT_ALCANCE ||
                   V_SSEPARADOR_TXT || V_DAT_CODPAISO || V_SSEPARADOR_TXT || V_DAT_CODPAISD || V_SSEPARADOR_TXT || V_DAT_INTERMOPE || V_SSEPARADOR_TXT || V_DAT_FORMA || V_SSEPARADOR_TXT ||
                   V_DAT_INFORM || V_SSEPARADOR_TXT || V_TIPO || V_SSEPARADOR_TXT || V_ORIGEN;

          UTL_FILE.PUT_LINE(FILE_OUTPUT, WTEXT);
        END LOOP;

      ELSIF P_EST_REPORTES = 'VR' THEN

        VIDA_MAYORES(P_OPE, P_TC, P_MONTO, P_FECINI, P_FECFIN, V_C_TABLE);

        WTEXT := '';
        SETEAR_VARIABLES;

        LOOP
          FETCH V_C_TABLE
            INTO V_FILA,
                 V_OFICINA,
                 V_OPERACION,
                 V_INTERNO,
                 V_MODALIDAD,
                 V_OPE_UBIGEO,
                 V_OPE_FECHA,
                 V_OPE_HORA,
                 V_EJE_RELACION,
                 V_EJE_CONDICION,
                 V_EJE_TIPPER,
                 V_EJE_TIPDOC,
                 V_EJE_NUMDOC,
                 V_EJE_NUMRUC,
                 V_EJE_APEPAT,
                 V_EJE_APEMAT,
                 V_EJE_NOMBRES,
                 V_EJE_OCUPACION,
                 V_EJE_PAIS,
                 V_EJE_CARGO,
                 V_EJE_PEP,
                 V_EJE_DOMICILIO,
                 V_EJE_DEPART,
                 V_EJE_PROV,
                 V_EJE_DIST,
                 V_EJE_TELEFONO,
                 V_ORD_RELACION,
                 V_ORD_CONDICION,
                 V_ORD_TIPPER,
                 V_ORD_TIPDOC,
                 V_ORD_NUMDOC,
                 V_ORD_NUMRUC,
                 V_ORD_APEPAT,
                 V_ORD_APEMAT,
                 V_ORD_NOMBRES,
                 V_ORD_OCUPACION,
                 V_ORD_PAIS,
                 V_ORD_CARGO,
                 V_ORD_PEP,
                 V_ORD_DOMICILIO,
                 V_ORD_DEPART,
                 V_ORD_PROV,
                 V_ORD_DIST,
                 V_ORD_TELEFONO,
                 V_BEN_RELACION,
                 V_BEN_CONDICION,
                 V_BEN_TIP_PER,
                 V_BEN_TIP_DOC,
                 V_BEN_NUM_DOC,
                 V_BEN_NUM_RUC,
                 V_BEN_APEPAT,
                 V_BEN_APEMAT,
                 V_BEN_NOMBRES,
                 V_BEN_OCUPACION,
                 V_BEN_PAIS,
                 V_BEN_CARGO,
                 V_BEN_PEP,
                 V_BEN_DOMICILIO,
                 V_BEN_DEPART,
                 V_BEN_PROV,
                 V_BEN_DIST,
                 V_BEN_TELEFONO,
                 V_DAT_TIPFON,
                 V_DAT_TIPOPE,
                 V_DAT_DESOPE,
                 V_DAT_ORIFON,
                 V_DAT_MONOPE,
                 V_DAT_MONOPE_A,
                 V_DAT_MTOOPE,
                 V_DAT_MTOOPEA,
                 V_DAT_COD_ENT_INVO,
                 V_DAT_COD_TIP_CTAO,
                 V_DAT_COD_CTAO,
                 V_DAT_ENT_FNC_EXTO,
                 V_DAT_COD_ENT_INVB,
                 V_DAT_COD_TIP_CTAB,
                 V_DAT_COD_CTAB,
                 V_DAT_ENT_FNC_EXTB,
                 V_DAT_ALCANCE,
                 V_DAT_COD_PAISO,
                 V_DAT_COD_PAISD,
                 V_DAT_INTOPE,
                 V_DAT_FORMA,
                 V_DAT_INFORM,
                 V_ORIGEN;

          EXIT WHEN V_C_TABLE%NOTFOUND;

          --CUERPO DEL ARCHIVO
          WTEXT := V_FILA || V_SSEPARADOR_TXT || V_OFICINA || V_SSEPARADOR_TXT || V_OPERACION || V_SSEPARADOR_TXT || V_INTERNO || V_SSEPARADOR_TXT || V_MODALIDAD || V_SSEPARADOR_TXT || V_OPE_UBIGEO ||
                   V_SSEPARADOR_TXT || V_OPE_FECHA || V_SSEPARADOR_TXT || V_OPE_HORA || V_SSEPARADOR_TXT || V_EJE_RELACION || V_SSEPARADOR_TXT || V_EJE_CONDICION || V_SSEPARADOR_TXT || V_EJE_TIPPER ||
                   V_SSEPARADOR_TXT || V_EJE_TIPDOC || V_SSEPARADOR_TXT || V_EJE_NUMDOC || V_SSEPARADOR_TXT || V_EJE_NUMRUC || V_SSEPARADOR_TXT || V_EJE_APEPAT || V_SSEPARADOR_TXT || V_EJE_APEMAT ||
                   V_SSEPARADOR_TXT || V_EJE_NOMBRES || V_SSEPARADOR_TXT || V_EJE_OCUPACION || V_SSEPARADOR_TXT || V_EJE_PAIS || V_SSEPARADOR_TXT || V_EJE_CARGO || V_SSEPARADOR_TXT || V_EJE_PEP ||
                   V_SSEPARADOR_TXT || V_EJE_DOMICILIO || V_SSEPARADOR_TXT || V_EJE_DEPART || V_SSEPARADOR_TXT || V_EJE_PROV || V_SSEPARADOR_TXT || V_EJE_DIST || V_SSEPARADOR_TXT || V_EJE_TELEFONO ||
                   V_SSEPARADOR_TXT || V_ORD_RELACION || V_SSEPARADOR_TXT || V_ORD_CONDICION || V_SSEPARADOR_TXT || V_ORD_TIPPER || V_SSEPARADOR_TXT || V_ORD_TIPDOC || V_SSEPARADOR_TXT ||
                   V_ORD_NUMDOC || V_SSEPARADOR_TXT || V_ORD_NUMRUC || V_SSEPARADOR_TXT || V_ORD_APEPAT || V_SSEPARADOR_TXT || V_ORD_APEMAT || V_SSEPARADOR_TXT || V_ORD_NOMBRES || V_SSEPARADOR_TXT ||
                   V_ORD_OCUPACION || V_SSEPARADOR_TXT || V_ORD_PAIS || V_SSEPARADOR_TXT || V_ORD_CARGO || V_SSEPARADOR_TXT || V_ORD_PEP || V_SSEPARADOR_TXT || V_ORD_DIRECCION || V_SSEPARADOR_TXT ||
                   V_ORD_DOMICILIO || V_SSEPARADOR_TXT || V_UBIGEO || V_SSEPARADOR_TXT || V_ORD_DEPART || V_SSEPARADOR_TXT || V_ORD_PROV || V_SSEPARADOR_TXT || V_ORD_DIST || V_SSEPARADOR_TXT ||
                   V_ORD_TELEFONO || V_SSEPARADOR_TXT || V_BEN_RELACION || V_SSEPARADOR_TXT || V_BEN_CONDICION || V_SSEPARADOR_TXT || V_BEN_TIP_PER || V_SSEPARADOR_TXT || V_BEN_TIP_DOC ||
                   V_SSEPARADOR_TXT || V_BEN_NUM_DOC || V_SSEPARADOR_TXT || V_BEN_NUM_RUC || V_SSEPARADOR_TXT || V_BEN_APEPAT || V_SSEPARADOR_TXT || V_BEN_APEMAT || V_SSEPARADOR_TXT || V_BEN_NOMBRES ||
                   V_SSEPARADOR_TXT || V_BEN_OCUPACION || V_SSEPARADOR_TXT || V_BEN_PAIS || V_SSEPARADOR_TXT || V_BEN_CARGO || V_SSEPARADOR_TXT || V_BEN_PEP || V_SSEPARADOR_TXT || V_SSEPARADOR_TXT ||
                   V_BEN_DOMICILIO || V_SSEPARADOR_TXT || V_SSEPARADOR_TXT || V_BEN_DEPART || V_SSEPARADOR_TXT || V_BEN_PROV || V_SSEPARADOR_TXT || V_BEN_DIST || V_SSEPARADOR_TXT || V_BEN_TELEFONO ||
                   V_SSEPARADOR_TXT || V_DAT_TIPFON || V_SSEPARADOR_TXT || V_DAT_TIPOPE || V_SSEPARADOR_TXT || V_DAT_DESOPE || V_SSEPARADOR_TXT || V_DAT_ORIFON || V_SSEPARADOR_TXT || V_DAT_MONOPE ||
                   V_SSEPARADOR_TXT || V_DAT_MONOPE_A || V_SSEPARADOR_TXT || V_DAT_MTOOPE || V_SSEPARADOR_TXT || V_DAT_MTOOPEA || V_SSEPARADOR_TXT || V_DAT_COD_ENT_INVO || V_SSEPARADOR_TXT ||
                   V_DAT_COD_TIP_CTAO || V_SSEPARADOR_TXT || V_DAT_COD_CTAO || V_SSEPARADOR_TXT || V_DAT_ENT_FNC_EXTO || V_SSEPARADOR_TXT || V_DAT_COD_ENT_INVB || V_SSEPARADOR_TXT ||
                   V_DAT_COD_TIP_CTAB || V_SSEPARADOR_TXT || V_DAT_COD_CTAB || V_SSEPARADOR_TXT || V_DAT_ENT_FNC_EXTB || V_SSEPARADOR_TXT || V_DAT_ALCANCE || V_SSEPARADOR_TXT || V_DAT_COD_PAISO ||
                   V_SSEPARADOR_TXT || V_DAT_COD_PAISD || V_SSEPARADOR_TXT || V_DAT_INTOPE || V_SSEPARADOR_TXT || V_DAT_FORMA || V_SSEPARADOR_TXT || V_DAT_INFORM || V_SSEPARADOR_TXT || V_ORIGEN;

          UTL_FILE.PUT_LINE(FILE_OUTPUT, WTEXT);

        END LOOP;

        RENTAS_MAYORES(P_FECINI, P_FECFIN, P_MONTO, P_TC, V_C_TABLE);

        WTEXT := '';
        SETEAR_VARIABLES;

        LOOP
          FETCH V_C_TABLE
            INTO V_FILA,
                 V_OPERACION,
                 V_OFICINA,
                 V_INTERNO,
                 V_MODALIDAD,
                 V_OPE_UBIGEO,
                 V_OPE_FECHA,
                 V_OPE_HORA,
                 V_EJE_RELACION,
                 V_EJE_CONDICION,
                 V_EJE_TIPPER,
                 V_EJE_TIPDOC,
                 V_EJE_NUMDOC,
                 V_EJE_NUMRUC,
                 V_EJE_APEPAT,
                 V_EJE_APEMAT,
                 V_EJE_NOMBRES,
                 V_EJE_OCUPACION,
                 V_EJE_PAIS,
                 V_EJE_CARGO,
                 V_EJE_PEP,
                 V_EJE_DOMICILIO,
                 V_EJE_DEPART,
                 V_EJE_PROV,
                 V_EJE_DIST,
                 V_EJE_TELEFONO,
                 V_ORD_RELACION,
                 V_ORD_CONDICION,
                 V_ORD_TIPPER,
                 V_ORD_TIPDOC,
                 V_ORD_NUMDOC,
                 V_ORD_NUMRUC,
                 V_ORD_APEPAT,
                 V_ORD_APEMAT,
                 V_ORD_NOMBRES,
                 V_ORD_OCUPACION,
                 V_ORD_PAIS,
                 V_ORD_CARGO,
                 V_ORD_PEP,
                 V_ORD_DOMICILIO,
                 V_ORD_DEPART,
                 V_ORD_PROV,
                 V_ORD_DIST,
                 V_ORD_TELEFONO,
                 V_BEN_RELACION,
                 V_BEN_CONDICION,
                 V_BEN_TIPPER,
                 V_BEN_TIPDOC,
                 V_BEN_NUMDOC,
                 V_BEN_NUMRUC,
                 V_BEN_APEPAT,
                 V_BEN_APEMAT,
                 V_BEN_NOMBRES,
                 V_BEN_OCUPACION,
                 V_BEN_PAIS,
                 V_BEN_CARGO,
                 V_BEN_PEP,
                 V_BEN_DOMICILIO,
                 V_BEN_DEPART,
                 V_BEN_PROV,
                 V_BEN_DIST,
                 V_BEN_TELEFONO,
                 V_DAT_TIPFON,
                 V_DAT_TIPOPE,
                 V_DAT_DESOPE,
                 V_DAT_ORIFON,
                 V_DAT_MONOPE,
                 V_DAT_MONOPEA,
                 V_DAT_MTOOPE,
                 V_DAT_MTOOPEA,
                 V_DAT_COD_ENT_INVO,
                 V_DAT_COD_TIP_CTAO,
                 V_DAT_COD_CTAO,
                 V_DAT_ENT_FNC_EXTO,
                 V_DAT_COD_ENT_INVB,
                 V_DAT_COD_TIP_CTAB,
                 V_DAT_COD_CTAB,
                 V_DAT_ENT_FNC_EXTB,
                 V_DAT_ALCANCE,
                 V_DAT_CODPAISO,
                 V_DAT_CODPAISD,
                 V_DAT_INTERMOPE,
                 V_DAT_FORMA,
                 V_DAT_INFORM,
                 V_TIPO,
                 V_ORIGEN;

          EXIT WHEN V_C_TABLE%NOTFOUND;

          WTEXT := V_FILA || V_SSEPARADOR_TXT || V_OPERACION || V_SSEPARADOR_TXT || V_OFICINA || V_SSEPARADOR_TXT || V_INTERNO || V_SSEPARADOR_TXT || V_MODALIDAD || V_SSEPARADOR_TXT || V_OPE_UBIGEO ||
                   V_SSEPARADOR_TXT || V_OPE_FECHA || V_SSEPARADOR_TXT || V_OPE_HORA || V_SSEPARADOR_TXT || V_EJE_RELACION || V_SSEPARADOR_TXT || V_EJE_CONDICION || V_SSEPARADOR_TXT || V_EJE_TIPPER ||
                   V_SSEPARADOR_TXT || V_EJE_TIPDOC || V_SSEPARADOR_TXT || V_EJE_NUMDOC || V_SSEPARADOR_TXT || V_EJE_NUMRUC || V_SSEPARADOR_TXT || V_EJE_APEPAT || V_SSEPARADOR_TXT || V_EJE_APEMAT ||
                   V_SSEPARADOR_TXT || V_EJE_NOMBRES || V_SSEPARADOR_TXT || V_EJE_OCUPACION || V_SSEPARADOR_TXT || V_EJE_PAIS || V_SSEPARADOR_TXT || V_EJE_CARGO || V_SSEPARADOR_TXT || V_EJE_PEP ||
                   V_SSEPARADOR_TXT || V_EJE_DOMICILIO || V_SSEPARADOR_TXT || V_EJE_DEPART || V_SSEPARADOR_TXT || V_EJE_PROV || V_SSEPARADOR_TXT || V_EJE_DIST || V_SSEPARADOR_TXT || V_EJE_TELEFONO ||
                   V_SSEPARADOR_TXT || V_ORD_RELACION || V_SSEPARADOR_TXT || V_ORD_CONDICION || V_SSEPARADOR_TXT || V_ORD_TIPPER || V_SSEPARADOR_TXT || V_ORD_TIPDOC || V_SSEPARADOR_TXT ||
                   V_ORD_NUMDOC || V_SSEPARADOR_TXT || V_ORD_NUMRUC || V_SSEPARADOR_TXT || V_ORD_APEPAT || V_SSEPARADOR_TXT || V_ORD_APEMAT || V_SSEPARADOR_TXT || V_ORD_NOMBRES || V_SSEPARADOR_TXT ||
                   V_ORD_OCUPACION || V_SSEPARADOR_TXT || V_ORD_PAIS || V_SSEPARADOR_TXT || V_ORD_CARGO || V_SSEPARADOR_TXT || V_ORD_PEP || V_SSEPARADOR_TXT || V_ORD_DOMICILIO || V_SSEPARADOR_TXT ||
                   V_ORD_DEPART || V_SSEPARADOR_TXT || V_ORD_PROV || V_SSEPARADOR_TXT || V_ORD_DIST || V_SSEPARADOR_TXT || V_ORD_TELEFONO || V_SSEPARADOR_TXT || V_BEN_RELACION || V_SSEPARADOR_TXT ||
                   V_BEN_CONDICION || V_SSEPARADOR_TXT || V_BEN_TIPPER || V_SSEPARADOR_TXT || V_BEN_TIPDOC || V_SSEPARADOR_TXT || V_BEN_NUMDOC || V_SSEPARADOR_TXT || V_BEN_NUMRUC || V_SSEPARADOR_TXT ||
                   V_BEN_APEPAT || V_SSEPARADOR_TXT || V_BEN_APEMAT || V_SSEPARADOR_TXT || V_BEN_NOMBRES || V_SSEPARADOR_TXT || V_BEN_OCUPACION || V_SSEPARADOR_TXT || V_BEN_PAIS || V_SSEPARADOR_TXT ||
                   V_BEN_CARGO || V_SSEPARADOR_TXT || V_BEN_PEP || V_SSEPARADOR_TXT || V_BEN_DOMICILIO || V_SSEPARADOR_TXT || V_BEN_DEPART || V_SSEPARADOR_TXT || V_BEN_PROV || V_SSEPARADOR_TXT ||
                   V_BEN_DIST || V_SSEPARADOR_TXT || V_BEN_TELEFONO || V_SSEPARADOR_TXT || V_DAT_TIPFON || V_SSEPARADOR_TXT || V_DAT_TIPOPE || V_SSEPARADOR_TXT || V_DAT_DESOPE || V_SSEPARADOR_TXT ||
                   V_DAT_ORIFON || V_SSEPARADOR_TXT || V_DAT_MONOPE || V_SSEPARADOR_TXT || V_DAT_MONOPEA || V_SSEPARADOR_TXT || V_DAT_MTOOPE || V_SSEPARADOR_TXT || V_DAT_MTOOPEA || V_SSEPARADOR_TXT ||
                   V_DAT_COD_ENT_INVO || V_SSEPARADOR_TXT || V_DAT_COD_TIP_CTAO || V_SSEPARADOR_TXT || V_DAT_COD_CTAO || V_SSEPARADOR_TXT || V_DAT_ENT_FNC_EXTO || V_SSEPARADOR_TXT ||
                   V_DAT_COD_ENT_INVB || V_SSEPARADOR_TXT || V_DAT_COD_TIP_CTAB || V_SSEPARADOR_TXT || V_DAT_COD_CTAB || V_SSEPARADOR_TXT || V_DAT_ENT_FNC_EXTB || V_SSEPARADOR_TXT || V_DAT_ALCANCE ||
                   V_SSEPARADOR_TXT || V_DAT_CODPAISO || V_SSEPARADOR_TXT || V_DAT_CODPAISD || V_SSEPARADOR_TXT || V_DAT_INTERMOPE || V_SSEPARADOR_TXT || V_DAT_FORMA || V_SSEPARADOR_TXT ||
                   V_DAT_INFORM || V_SSEPARADOR_TXT || V_TIPO || V_SSEPARADOR_TXT || V_ORIGEN;

          UTL_FILE.PUT_LINE(FILE_OUTPUT, WTEXT);
        END LOOP;

      ELSIF P_EST_REPORTES = 'SVR' THEN

        SINIESTROS_MAYORES(P_OPE, P_TC, P_MONTO, P_FECINI, P_FECFIN, V_C_TABLE);

        WTEXT := '';
        SETEAR_VARIABLES;

        LOOP
          FETCH V_C_TABLE

            INTO V_FILA,
                 V_OFICINA,
                 V_OPERACION,
                 V_INTERNO,
                 V_MODALIDAD,
                 V_OPE_UBIGEO,
                 V_OPE_FECHA,
                 V_OPE_HORA,
                 V_EJE_RELACION,
                 V_EJE_CONDICION,
                 V_EJE_TIPPER,
                 V_EJE_TIPDOC,
                 V_EJE_NUMDOC,
                 V_EJE_NUMRUC,
                 V_EJE_APEPAT,
                 V_EJE_APEMAT,
                 V_EJE_NOMBRES,
                 V_EJE_OCUPACION,
                 V_EJE_PAIS,
                 V_EJE_CARGO,
                 V_EJE_PEP,
                 V_EJE_DOMICILIO,
                 V_EJE_DEPART,
                 V_EJE_PROV,
                 V_EJE_DIST,
                 V_EJE_TELEFONO,
                 V_ORD_RELACION,
                 V_ORD_CONDICION,
                 V_ORD_TIPPER,
                 V_ORD_TIPDOC,
                 V_ORD_NUMDOC,
                 V_ORD_NUMRUC,
                 V_ORD_APEPAT,
                 V_ORD_APEMAT,
                 V_ORD_NOMBRES,
                 V_ORD_OCUPACION,
                 V_ORD_PAIS,
                 V_ORD_CARGO,
                 V_ORD_PEP,
                 V_ORD_DOMICILIO,
                 V_ORD_DEPART,
                 V_ORD_PROV,
                 V_ORD_DIST,

                 V_ORD_TELEFONO,
                 V_BEN_RELACION,
                 V_BEN_CONDICION,
                 V_BEN_TIP_PER,
                 V_BEN_TIP_DOC,
                 V_BEN_NUM_DOC,
                 V_BEN_NUM_RUC,
                 V_BEN_APEPAT,
                 V_BEN_APEMAT,
                 V_BEN_NOMBRES,
                 V_BEN_OCUPACION,
                 V_BEN_PAIS,
                 V_BEN_CARGO,
                 V_BEN_PEP,

                 V_BEN_DOMICILIO,

                 V_BEN_DEPART,
                 V_BEN_PROV,
                 V_BEN_DIST,
                 V_BEN_TELEFONO,
                 V_DAT_TIPFON,
                 V_DAT_TIPOPE,
                 V_DAT_DESOPE,
                 V_DAT_ORIFON,
                 V_DAT_MONOPE,
                 V_DAT_MONOPE_A,
                 V_DAT_MTOOPE,
                 V_DAT_MTOOPEA,
                 V_DAT_COD_ENT_INVO,
                 V_DAT_COD_TIP_CTAO,
                 V_DAT_COD_CTAO,
                 V_DAT_ENT_FNC_EXTO,
                 V_DAT_COD_ENT_INVB,
                 V_DAT_COD_TIP_CTAB,
                 V_DAT_COD_CTAB,
                 V_DAT_ENT_FNC_EXTB,
                 V_DAT_ALCANCE,
                 V_DAT_COD_PAISO,
                 V_DAT_COD_PAISD,
                 V_DAT_INTOPE,
                 V_DAT_FORMA,
                 V_DAT_INFORM,
                 V_ORIGEN;

          EXIT WHEN V_C_TABLE%NOTFOUND;

          --CUERPO DEL ARCHIVO
          WTEXT := V_FILA || V_SSEPARADOR_TXT || V_OFICINA || V_SSEPARADOR_TXT || V_OPERACION || V_SSEPARADOR_TXT || V_INTERNO || V_SSEPARADOR_TXT || V_MODALIDAD || V_SSEPARADOR_TXT || V_OPE_UBIGEO ||
                   V_SSEPARADOR_TXT || V_OPE_FECHA || V_SSEPARADOR_TXT || V_OPE_HORA || V_SSEPARADOR_TXT || V_EJE_RELACION || V_SSEPARADOR_TXT || V_EJE_CONDICION || V_SSEPARADOR_TXT || V_EJE_TIPPER ||
                   V_SSEPARADOR_TXT || V_EJE_TIPDOC || V_SSEPARADOR_TXT || V_EJE_NUMDOC || V_SSEPARADOR_TXT || V_EJE_NUMRUC || V_SSEPARADOR_TXT || V_EJE_APEPAT || V_SSEPARADOR_TXT || V_EJE_APEMAT ||
                   V_SSEPARADOR_TXT || V_EJE_NOMBRES || V_SSEPARADOR_TXT || V_EJE_OCUPACION || V_SSEPARADOR_TXT || V_EJE_PAIS || V_SSEPARADOR_TXT || V_EJE_CARGO || V_SSEPARADOR_TXT || V_EJE_PEP ||
                   V_SSEPARADOR_TXT || V_EJE_DOMICILIO || V_SSEPARADOR_TXT || V_EJE_DEPART || V_SSEPARADOR_TXT || V_EJE_PROV || V_SSEPARADOR_TXT || V_EJE_DIST || V_SSEPARADOR_TXT || V_EJE_TELEFONO ||
                   V_SSEPARADOR_TXT || V_ORD_RELACION || V_SSEPARADOR_TXT || V_ORD_CONDICION || V_SSEPARADOR_TXT || V_ORD_TIPPER || V_SSEPARADOR_TXT || V_ORD_TIPDOC || V_SSEPARADOR_TXT ||
                   V_ORD_NUMDOC || V_SSEPARADOR_TXT || V_ORD_NUMRUC || V_SSEPARADOR_TXT || V_ORD_APEPAT || V_SSEPARADOR_TXT || V_ORD_APEMAT || V_SSEPARADOR_TXT || V_ORD_NOMBRES || V_SSEPARADOR_TXT ||
                   V_ORD_OCUPACION || V_SSEPARADOR_TXT || V_ORD_PAIS || V_SSEPARADOR_TXT || V_ORD_CARGO || V_SSEPARADOR_TXT || V_ORD_PEP || V_SSEPARADOR_TXT || V_ORD_DOMICILIO || V_SSEPARADOR_TXT ||
                   V_ORD_DEPART || V_SSEPARADOR_TXT || V_ORD_PROV || V_SSEPARADOR_TXT || V_ORD_DIST || V_SSEPARADOR_TXT || V_SSEPARADOR_TXT || V_ORD_TELEFONO || V_SSEPARADOR_TXT || V_BEN_RELACION ||
                   V_SSEPARADOR_TXT || V_BEN_CONDICION || V_SSEPARADOR_TXT || V_BEN_TIP_PER || V_SSEPARADOR_TXT || V_BEN_TIP_DOC || V_SSEPARADOR_TXT || V_BEN_NUM_DOC || V_SSEPARADOR_TXT ||
                   V_BEN_NUM_RUC || V_SSEPARADOR_TXT || V_BEN_APEPAT || V_SSEPARADOR_TXT || V_BEN_APEMAT || V_SSEPARADOR_TXT || V_BEN_NOMBRES || V_SSEPARADOR_TXT || V_BEN_OCUPACION ||
                   V_SSEPARADOR_TXT || V_BEN_PAIS || V_SSEPARADOR_TXT || V_BEN_CARGO || V_SSEPARADOR_TXT || V_BEN_PEP || V_SSEPARADOR_TXT || V_SSEPARADOR_TXT || V_BEN_DOMICILIO || V_SSEPARADOR_TXT ||
                   V_SSEPARADOR_TXT || V_BEN_DEPART || V_SSEPARADOR_TXT || V_BEN_PROV || V_SSEPARADOR_TXT || V_BEN_DIST || V_SSEPARADOR_TXT || V_BEN_TELEFONO || V_SSEPARADOR_TXT || V_DAT_TIPFON ||
                   V_SSEPARADOR_TXT || V_DAT_TIPOPE || V_SSEPARADOR_TXT || V_DAT_DESOPE || V_SSEPARADOR_TXT || V_DAT_ORIFON || V_SSEPARADOR_TXT || V_DAT_MONOPE || V_SSEPARADOR_TXT || V_DAT_MONOPE_A ||
                   V_SSEPARADOR_TXT || V_DAT_MTOOPE || V_SSEPARADOR_TXT || V_DAT_MTOOPEA || V_SSEPARADOR_TXT || V_DAT_COD_ENT_INVO || V_SSEPARADOR_TXT || V_DAT_COD_TIP_CTAO || V_SSEPARADOR_TXT ||
                   V_DAT_COD_CTAO || V_SSEPARADOR_TXT || V_DAT_ENT_FNC_EXTO || V_SSEPARADOR_TXT || V_DAT_COD_ENT_INVB || V_SSEPARADOR_TXT || V_DAT_COD_TIP_CTAB || V_SSEPARADOR_TXT || V_DAT_COD_CTAB ||
                   V_SSEPARADOR_TXT || V_DAT_ENT_FNC_EXTB || V_SSEPARADOR_TXT || V_DAT_ALCANCE || V_SSEPARADOR_TXT || V_DAT_COD_PAISO || V_SSEPARADOR_TXT || V_DAT_COD_PAISD || V_SSEPARADOR_TXT ||
                   V_DAT_INTOPE || V_SSEPARADOR_TXT || V_DAT_FORMA || V_SSEPARADOR_TXT || V_DAT_INFORM || V_SSEPARADOR_TXT || V_ORIGEN;

          UTL_FILE.PUT_LINE(FILE_OUTPUT, WTEXT);
        END LOOP;

        RENTAS_MAYORES(P_FECINI, P_FECFIN, P_MONTO, P_TC, V_C_TABLE);

        WTEXT := '';
        SETEAR_VARIABLES;

        LOOP
          FETCH V_C_TABLE
            INTO V_FILA,
                 V_OPERACION,
                 V_OFICINA,
                 V_INTERNO,
                 V_MODALIDAD,
                 V_OPE_UBIGEO,
                 V_OPE_FECHA,
                 V_OPE_HORA,
                 V_EJE_RELACION,
                 V_EJE_CONDICION,
                 V_EJE_TIPPER,
                 V_EJE_TIPDOC,
                 V_EJE_NUMDOC,
                 V_EJE_NUMRUC,
                 V_EJE_APEPAT,
                 V_EJE_APEMAT,
                 V_EJE_NOMBRES,
                 V_EJE_OCUPACION,
                 V_EJE_PAIS,
                 V_EJE_CARGO,
                 V_EJE_PEP,
                 V_EJE_DOMICILIO,
                 V_EJE_DEPART,
                 V_EJE_PROV,
                 V_EJE_DIST,
                 V_EJE_TELEFONO,
                 V_ORD_RELACION,
                 V_ORD_CONDICION,
                 V_ORD_TIPPER,
                 V_ORD_TIPDOC,
                 V_ORD_NUMDOC,
                 V_ORD_NUMRUC,
                 V_ORD_APEPAT,
                 V_ORD_APEMAT,
                 V_ORD_NOMBRES,
                 V_ORD_OCUPACION,
                 V_ORD_PAIS,
                 V_ORD_CARGO,
                 V_ORD_PEP,
                 V_ORD_DOMICILIO,
                 V_ORD_DEPART,
                 V_ORD_PROV,
                 V_ORD_DIST,
                 V_ORD_TELEFONO,
                 V_BEN_RELACION,
                 V_BEN_CONDICION,
                 V_BEN_TIPPER,
                 V_BEN_TIPDOC,
                 V_BEN_NUMDOC,
                 V_BEN_NUMRUC,
                 V_BEN_APEPAT,
                 V_BEN_APEMAT,
                 V_BEN_NOMBRES,
                 V_BEN_OCUPACION,
                 V_BEN_PAIS,
                 V_BEN_CARGO,
                 V_BEN_PEP,
                 V_BEN_DOMICILIO,
                 V_BEN_DEPART,
                 V_BEN_PROV,
                 V_BEN_DIST,
                 V_BEN_TELEFONO,
                 V_DAT_TIPFON,
                 V_DAT_TIPOPE,
                 V_DAT_DESOPE,
                 V_DAT_ORIFON,
                 V_DAT_MONOPE,
                 V_DAT_MONOPEA,
                 V_DAT_MTOOPE,
                 V_DAT_MTOOPEA,
                 V_DAT_COD_ENT_INVO,
                 V_DAT_COD_TIP_CTAO,
                 V_DAT_COD_CTAO,
                 V_DAT_ENT_FNC_EXTO,
                 V_DAT_COD_ENT_INVB,
                 V_DAT_COD_TIP_CTAB,
                 V_DAT_COD_CTAB,
                 V_DAT_ENT_FNC_EXTB,
                 V_DAT_ALCANCE,
                 V_DAT_CODPAISO,
                 V_DAT_CODPAISD,
                 V_DAT_INTERMOPE,
                 V_DAT_FORMA,
                 V_DAT_INFORM,
                 V_TIPO,
                 V_ORIGEN;

          EXIT WHEN V_C_TABLE%NOTFOUND;

          WTEXT := V_FILA || V_SSEPARADOR_TXT || V_OPERACION || V_SSEPARADOR_TXT || V_OFICINA || V_SSEPARADOR_TXT || V_INTERNO || V_SSEPARADOR_TXT || V_MODALIDAD || V_SSEPARADOR_TXT || V_OPE_UBIGEO ||
                   V_SSEPARADOR_TXT || V_OPE_FECHA || V_SSEPARADOR_TXT || V_OPE_HORA || V_SSEPARADOR_TXT || V_EJE_RELACION || V_SSEPARADOR_TXT || V_EJE_CONDICION || V_SSEPARADOR_TXT || V_EJE_TIPPER ||
                   V_SSEPARADOR_TXT || V_EJE_TIPDOC || V_SSEPARADOR_TXT || V_EJE_NUMDOC || V_SSEPARADOR_TXT || V_EJE_NUMRUC || V_SSEPARADOR_TXT || V_EJE_APEPAT || V_SSEPARADOR_TXT || V_EJE_APEMAT ||
                   V_SSEPARADOR_TXT || V_EJE_NOMBRES || V_SSEPARADOR_TXT || V_EJE_OCUPACION || V_SSEPARADOR_TXT || V_EJE_PAIS || V_SSEPARADOR_TXT || V_EJE_CARGO || V_SSEPARADOR_TXT || V_EJE_PEP ||
                   V_SSEPARADOR_TXT || V_EJE_DOMICILIO || V_SSEPARADOR_TXT || V_EJE_DEPART || V_SSEPARADOR_TXT || V_EJE_PROV || V_SSEPARADOR_TXT || V_EJE_DIST || V_SSEPARADOR_TXT || V_EJE_TELEFONO ||
                   V_SSEPARADOR_TXT || V_ORD_RELACION || V_SSEPARADOR_TXT || V_ORD_CONDICION || V_SSEPARADOR_TXT || V_ORD_TIPPER || V_SSEPARADOR_TXT || V_ORD_TIPDOC || V_SSEPARADOR_TXT ||
                   V_ORD_NUMDOC || V_SSEPARADOR_TXT || V_ORD_NUMRUC || V_SSEPARADOR_TXT || V_ORD_APEPAT || V_SSEPARADOR_TXT || V_ORD_APEMAT || V_SSEPARADOR_TXT || V_ORD_NOMBRES || V_SSEPARADOR_TXT ||
                   V_ORD_OCUPACION || V_SSEPARADOR_TXT || V_ORD_PAIS || V_SSEPARADOR_TXT || V_ORD_CARGO || V_SSEPARADOR_TXT || V_ORD_PEP || V_SSEPARADOR_TXT || V_ORD_DOMICILIO || V_SSEPARADOR_TXT ||
                   V_ORD_DEPART || V_SSEPARADOR_TXT || V_ORD_PROV || V_SSEPARADOR_TXT || V_ORD_DIST || V_SSEPARADOR_TXT || V_ORD_TELEFONO || V_SSEPARADOR_TXT || V_BEN_RELACION || V_SSEPARADOR_TXT ||
                   V_BEN_CONDICION || V_SSEPARADOR_TXT || V_BEN_TIPPER || V_SSEPARADOR_TXT || V_BEN_TIPDOC || V_SSEPARADOR_TXT || V_BEN_NUMDOC || V_SSEPARADOR_TXT || V_BEN_NUMRUC || V_SSEPARADOR_TXT ||
                   V_BEN_APEPAT || V_SSEPARADOR_TXT || V_BEN_APEMAT || V_SSEPARADOR_TXT || V_BEN_NOMBRES || V_SSEPARADOR_TXT || V_BEN_OCUPACION || V_SSEPARADOR_TXT || V_BEN_PAIS || V_SSEPARADOR_TXT ||
                   V_BEN_CARGO || V_SSEPARADOR_TXT || V_BEN_PEP || V_SSEPARADOR_TXT || V_BEN_DOMICILIO || V_SSEPARADOR_TXT || V_BEN_DEPART || V_SSEPARADOR_TXT || V_BEN_PROV || V_SSEPARADOR_TXT ||
                   V_BEN_DIST || V_SSEPARADOR_TXT || V_BEN_TELEFONO || V_SSEPARADOR_TXT || V_DAT_TIPFON || V_SSEPARADOR_TXT || V_DAT_TIPOPE || V_SSEPARADOR_TXT || V_DAT_DESOPE || V_SSEPARADOR_TXT ||
                   V_DAT_ORIFON || V_SSEPARADOR_TXT || V_DAT_MONOPE || V_SSEPARADOR_TXT || V_DAT_MONOPEA || V_SSEPARADOR_TXT || V_DAT_MTOOPE || V_SSEPARADOR_TXT || V_DAT_MTOOPEA || V_SSEPARADOR_TXT ||
                   V_DAT_COD_ENT_INVO || V_SSEPARADOR_TXT || V_DAT_COD_TIP_CTAO || V_SSEPARADOR_TXT || V_DAT_COD_CTAO || V_SSEPARADOR_TXT || V_DAT_ENT_FNC_EXTO || V_SSEPARADOR_TXT ||
                   V_DAT_COD_ENT_INVB || V_SSEPARADOR_TXT || V_DAT_COD_TIP_CTAB || V_SSEPARADOR_TXT || V_DAT_COD_CTAB || V_SSEPARADOR_TXT || V_DAT_ENT_FNC_EXTB || V_SSEPARADOR_TXT || V_DAT_ALCANCE ||
                   V_SSEPARADOR_TXT || V_DAT_CODPAISO || V_SSEPARADOR_TXT || V_DAT_CODPAISD || V_SSEPARADOR_TXT || V_DAT_INTERMOPE || V_SSEPARADOR_TXT || V_DAT_FORMA || V_SSEPARADOR_TXT ||
                   V_DAT_INFORM || V_SSEPARADOR_TXT || V_TIPO || V_SSEPARADOR_TXT || V_ORIGEN;

          UTL_FILE.PUT_LINE(FILE_OUTPUT, WTEXT);
        END LOOP;

        VIDA_MAYORES(P_OPE, P_TC, P_MONTO, P_FECINI, P_FECFIN, V_C_TABLE);

        WTEXT := '';
        SETEAR_VARIABLES;

        LOOP
          FETCH V_C_TABLE
            INTO V_FILA,
                 V_OFICINA,
                 V_OPERACION,
                 V_INTERNO,
                 V_MODALIDAD,
                 V_OPE_UBIGEO,
                 V_OPE_FECHA,
                 V_OPE_HORA,
                 V_EJE_RELACION,
                 V_EJE_CONDICION,
                 V_EJE_TIPPER,
                 V_EJE_TIPDOC,
                 V_EJE_NUMDOC,
                 V_EJE_NUMRUC,
                 V_EJE_APEPAT,
                 V_EJE_APEMAT,
                 V_EJE_NOMBRES,
                 V_EJE_OCUPACION,
                 V_EJE_PAIS,
                 V_EJE_CARGO,
                 V_EJE_PEP,
                 V_EJE_DOMICILIO,
                 V_EJE_DEPART,
                 V_EJE_PROV,
                 V_EJE_DIST,
                 V_EJE_TELEFONO,
                 V_ORD_RELACION,
                 V_ORD_CONDICION,
                 V_ORD_TIPPER,
                 V_ORD_TIPDOC,
                 V_ORD_NUMDOC,
                 V_ORD_NUMRUC,
                 V_ORD_APEPAT,
                 V_ORD_APEMAT,
                 V_ORD_NOMBRES,
                 V_ORD_OCUPACION,
                 V_ORD_PAIS,
                 V_ORD_CARGO,
                 V_ORD_PEP,
                 V_ORD_DOMICILIO,
                 V_ORD_DEPART,
                 V_ORD_PROV,
                 V_ORD_DIST,
                 V_ORD_TELEFONO,
                 V_BEN_RELACION,
                 V_BEN_CONDICION,
                 V_BEN_TIP_PER,
                 V_BEN_TIP_DOC,
                 V_BEN_NUM_DOC,
                 V_BEN_NUM_RUC,
                 V_BEN_APEPAT,
                 V_BEN_APEMAT,
                 V_BEN_NOMBRES,
                 V_BEN_OCUPACION,
                 V_BEN_PAIS,
                 V_BEN_CARGO,
                 V_BEN_PEP,
                 V_BEN_DOMICILIO,
                 V_BEN_DEPART,
                 V_BEN_PROV,
                 V_BEN_DIST,
                 V_BEN_TELEFONO,
                 V_DAT_TIPFON,
                 V_DAT_TIPOPE,
                 V_DAT_DESOPE,
                 V_DAT_ORIFON,
                 V_DAT_MONOPE,
                 V_DAT_MONOPE_A,
                 V_DAT_MTOOPE,
                 V_DAT_MTOOPEA,
                 V_DAT_COD_ENT_INVO,
                 V_DAT_COD_TIP_CTAO,
                 V_DAT_COD_CTAO,
                 V_DAT_ENT_FNC_EXTO,
                 V_DAT_COD_ENT_INVB,
                 V_DAT_COD_TIP_CTAB,
                 V_DAT_COD_CTAB,
                 V_DAT_ENT_FNC_EXTB,
                 V_DAT_ALCANCE,
                 V_DAT_COD_PAISO,
                 V_DAT_COD_PAISD,
                 V_DAT_INTOPE,
                 V_DAT_FORMA,
                 V_DAT_INFORM,
                 V_ORIGEN;

          EXIT WHEN V_C_TABLE%NOTFOUND;

          --CUERPO DEL ARCHIVO
          WTEXT := V_FILA || V_SSEPARADOR_TXT || V_OFICINA || V_SSEPARADOR_TXT || V_OPERACION || V_SSEPARADOR_TXT || V_INTERNO || V_SSEPARADOR_TXT || V_MODALIDAD || V_SSEPARADOR_TXT || V_OPE_UBIGEO ||
                   V_SSEPARADOR_TXT || V_OPE_FECHA || V_SSEPARADOR_TXT || V_OPE_HORA || V_SSEPARADOR_TXT || V_EJE_RELACION || V_SSEPARADOR_TXT || V_EJE_CONDICION || V_SSEPARADOR_TXT || V_EJE_TIPPER ||
                   V_SSEPARADOR_TXT || V_EJE_TIPDOC || V_SSEPARADOR_TXT || V_EJE_NUMDOC || V_SSEPARADOR_TXT || V_EJE_NUMRUC || V_SSEPARADOR_TXT || V_EJE_APEPAT || V_SSEPARADOR_TXT || V_EJE_APEMAT ||
                   V_SSEPARADOR_TXT || V_EJE_NOMBRES || V_SSEPARADOR_TXT || V_EJE_OCUPACION || V_SSEPARADOR_TXT || V_EJE_PAIS || V_SSEPARADOR_TXT || V_EJE_CARGO || V_SSEPARADOR_TXT || V_EJE_PEP ||
                   V_SSEPARADOR_TXT || V_EJE_DOMICILIO || V_SSEPARADOR_TXT || V_EJE_DEPART || V_SSEPARADOR_TXT || V_EJE_PROV || V_SSEPARADOR_TXT || V_EJE_DIST || V_SSEPARADOR_TXT || V_EJE_TELEFONO ||
                   V_SSEPARADOR_TXT || V_ORD_RELACION || V_SSEPARADOR_TXT || V_ORD_CONDICION || V_SSEPARADOR_TXT || V_ORD_TIPPER || V_SSEPARADOR_TXT || V_ORD_TIPDOC || V_SSEPARADOR_TXT ||
                   V_ORD_NUMDOC || V_SSEPARADOR_TXT || V_ORD_NUMRUC || V_SSEPARADOR_TXT || V_ORD_APEPAT || V_SSEPARADOR_TXT || V_ORD_APEMAT || V_SSEPARADOR_TXT || V_ORD_NOMBRES || V_SSEPARADOR_TXT ||
                   V_ORD_OCUPACION || V_SSEPARADOR_TXT || V_ORD_PAIS || V_SSEPARADOR_TXT || V_ORD_CARGO || V_SSEPARADOR_TXT || V_ORD_PEP || V_SSEPARADOR_TXT || V_ORD_DIRECCION || V_SSEPARADOR_TXT ||
                   V_ORD_DOMICILIO || V_SSEPARADOR_TXT || V_UBIGEO || V_SSEPARADOR_TXT || V_ORD_DEPART || V_SSEPARADOR_TXT || V_ORD_PROV || V_SSEPARADOR_TXT || V_ORD_DIST || V_SSEPARADOR_TXT ||
                   V_ORD_TELEFONO || V_SSEPARADOR_TXT || V_BEN_RELACION || V_SSEPARADOR_TXT || V_BEN_CONDICION || V_SSEPARADOR_TXT || V_BEN_TIP_PER || V_SSEPARADOR_TXT || V_BEN_TIP_DOC ||
                   V_SSEPARADOR_TXT || V_BEN_NUM_DOC || V_SSEPARADOR_TXT || V_BEN_NUM_RUC || V_SSEPARADOR_TXT || V_BEN_APEPAT || V_SSEPARADOR_TXT || V_BEN_APEMAT || V_SSEPARADOR_TXT || V_BEN_NOMBRES ||
                   V_SSEPARADOR_TXT || V_BEN_OCUPACION || V_SSEPARADOR_TXT || V_BEN_PAIS || V_SSEPARADOR_TXT || V_BEN_CARGO || V_SSEPARADOR_TXT || V_BEN_PEP || V_SSEPARADOR_TXT || V_SSEPARADOR_TXT ||
                   V_BEN_DOMICILIO || V_SSEPARADOR_TXT || V_SSEPARADOR_TXT || V_BEN_DEPART || V_SSEPARADOR_TXT || V_BEN_PROV || V_SSEPARADOR_TXT || V_BEN_DIST || V_SSEPARADOR_TXT || V_BEN_TELEFONO ||
                   V_SSEPARADOR_TXT || V_DAT_TIPFON || V_SSEPARADOR_TXT || V_DAT_TIPOPE || V_SSEPARADOR_TXT || V_DAT_DESOPE || V_SSEPARADOR_TXT || V_DAT_ORIFON || V_SSEPARADOR_TXT || V_DAT_MONOPE ||
                   V_SSEPARADOR_TXT || V_DAT_MONOPE_A || V_SSEPARADOR_TXT || V_DAT_MTOOPE || V_SSEPARADOR_TXT || V_DAT_MTOOPEA || V_SSEPARADOR_TXT || V_DAT_COD_ENT_INVO || V_SSEPARADOR_TXT ||
                   V_DAT_COD_TIP_CTAO || V_SSEPARADOR_TXT || V_DAT_COD_CTAO || V_SSEPARADOR_TXT || V_DAT_ENT_FNC_EXTO || V_SSEPARADOR_TXT || V_DAT_COD_ENT_INVB || V_SSEPARADOR_TXT ||
                   V_DAT_COD_TIP_CTAB || V_SSEPARADOR_TXT || V_DAT_COD_CTAB || V_SSEPARADOR_TXT || V_DAT_ENT_FNC_EXTB || V_SSEPARADOR_TXT || V_DAT_ALCANCE || V_SSEPARADOR_TXT || V_DAT_COD_PAISO ||
                   V_SSEPARADOR_TXT || V_DAT_COD_PAISD || V_SSEPARADOR_TXT || V_DAT_INTOPE || V_SSEPARADOR_TXT || V_DAT_FORMA || V_SSEPARADOR_TXT || V_DAT_INFORM || V_SSEPARADOR_TXT || V_ORIGEN;

          UTL_FILE.PUT_LINE(FILE_OUTPUT, WTEXT);

        END LOOP;

      END IF;

      UTL_FILE.FCLOSE(FILE_OUTPUT);
      P_SRUTA := '\\' || P_SRUTA || '\' || WRUTA || '\' || WNOMARCH;
      CLOSE V_C_TABLE;
    END IF;

    UPDATE LAFT.TBL_TRX_MONITOREO_REP_SBS
       SET SMENSAJE    = P_SMESSAGE,
           NSTATUSPROC = 2,
           DFINPROC    = SYSDATE
     WHERE SID = V_SID;

  EXCEPTION
    WHEN OTHERS THEN
      P_NCODE    := 1;
      P_SMESSAGE := SQLERRM || CHR(13) || DBMS_UTILITY.FORMAT_ERROR_BACKTRACE;

      UPDATE LAFT.TBL_TRX_MONITOREO_REP_SBS
         SET SMENSAJE    = P_SMESSAGE,
             NSTATUSPROC = 3,
             DFINPROC    = SYSDATE
       WHERE SID = V_SID;

  END;
*/
  PROCEDURE SP_OBT_TIPO_CAMBIO(P_TIPOCAMBIO OUT NUMBER) IS
  BEGIN
    P_TIPOCAMBIO := 3.5;
  END;

  PROCEDURE SP_OBT_RUTA(P_ID   VARCHAR2,
                        P_RUTA OUT VARCHAR2) IS

    V_RUTA VARCHAR2(100);
    V_ID   VARCHAR2(100);

  BEGIN
    V_ID   := P_ID;
    V_RUTA := '\\172.23.2.145\CONCILIACIONES\REPORTE_';
    P_RUTA := V_RUTA;
  END;
/*
  PROCEDURE SP_MONITOREO_REP_SBS(P_FECEJE_INI  VARCHAR2,
                                 P_FECEJE_FIN  VARCHAR2,
                                 P_SID         VARCHAR,
                                 P_TI_BUSQUEDA VARCHAR2,
                                 P_NCODE       OUT NUMBER,
                                 P_SMESSAGE    OUT VARCHAR2,
                                 C_TABLE       OUT SYS_REFCURSOR) IS
  BEGIN
    P_NCODE := 0;

    OPEN C_TABLE FOR
      SELECT NULL SID
        FROM DUAL;

    IF P_TI_BUSQUEDA = 1 THEN
      IF P_FECEJE_INI IS NULL THEN
        P_NCODE    := 1;
        P_SMESSAGE := 'Debe ingresar obligatoriamente la fecha inicial que ejecutó el reporte';
        RETURN;
      END IF;
      IF P_FECEJE_FIN IS NULL THEN
        P_NCODE    := 1;
        P_SMESSAGE := 'Debe ingresar obligatoriamente la fecha final que ejecutó el reporte';
        RETURN;
      END IF;

      IF TO_DATE(P_FECEJE_FIN, 'DD/MM/YYYY') < TO_DATE(P_FECEJE_INI, 'DD/MM/YYYY') THEN
        P_NCODE    := 1;
        P_SMESSAGE := 'La fecha inicial no puede ser mayor que la fecha final';
        RETURN;
      END IF;
    ELSE
      IF P_SID IS NULL THEN
        P_NCODE    := 1;
        P_SMESSAGE := 'Debe ingresar el id del proceso obligatoriamente';
        RETURN;
      END IF;
    END IF;

    OPEN C_TABLE FOR
      SELECT SID,
             DINIREP,
             DFINREP,
             NTIPCAMBIO,
             DECODE(STIPOPE, 1, 'ÚNICAS', 2, 'MÚLTIPLES', 3, 'ÚNICAS Y MÚLTIPLES') AS STIPOPE,
             DECODE(SORIGEN, 'S', 'SINIESTROS', 'R', 'RENTAS', 'V', 'VIDA', 'SR', 'SINIESTROS y RENTAS', 'SV', 'SINIESTROS Y VIDA', 'VR', 'VIDA y RENTAS', 'SVR', 'SINIESTROS Y VIDA Y RENTAS') AS SORIGEN,
             TRIM(ST.SDESCRIPTION) AS SDESCRIPTION \*,
                                                                                                             DINIPROC AS FE_INI_PROCESO,
                                                                                                             DFINPROC AS FE_FIN_PROCESO*\
        FROM LAFT.TBL_TRX_MONITOREO_REP_SBS SBS,
             TBL_TMAE_STATUS_PROC      ST
       WHERE (P_TI_BUSQUEDA = '0' OR TRUNC(SBS.DCOMPDATE) BETWEEN P_FECEJE_INI AND P_FECEJE_FIN)
         AND (P_TI_BUSQUEDA = '1' OR P_SID = SBS.SID)
         AND ST.NSTATUSPROC = SBS.NSTATUSPROC;
  EXCEPTION
    WHEN OTHERS THEN
      P_NCODE    := 1;
      P_SMESSAGE := SQLERRM || CHR(13) || DBMS_UTILITY.FORMAT_ERROR_BACKTRACE;
  END;
*/


/*
  PROCEDURE SINIESTROS_MAYORES_SIN(P_OPE    NUMBER,
                               P_TC     NUMBER,
                               P_MONTO  NUMBER,
                               P_FECINI VARCHAR2,
                               P_FECFIN VARCHAR2,
                               C_TABLE  OUT MYCURSOR) IS
                               
   V_COUNT INTEGER;
   V_TABLE_LISTA_REPORTE_SBS T_TABLE_LISTA_REPORTE_SBS := T_TABLE_LISTA_REPORTE_SBS();                              
   V_CLI_HABITUAL VARCHAR2(1) := '';
   V_SCLIENT_ORD CLIENT.SCLIENT%TYPE;
    V_SPHONE_ORD  PHONES.SPHONE%TYPE;

    V_SCLIENT_AD        CLIENT.SCLIENT%TYPE;
    V_SDIRECCION_ORD    ADDRESS_CLIENT.SDESDIREBUSQ%TYPE;
    V_STI_DIRE          ADDRESS_CLIENT.STI_DIRE%TYPE;
    V_SNOM_DIRECCION    ADDRESS_CLIENT.SNOM_DIRECCION%TYPE;
    V_SNUM_DIRECCION    ADDRESS_CLIENT.SNUM_DIRECCION%TYPE;
    V_STI_BLOCKCHALET   ADDRESS_CLIENT.STI_BLOCKCHALET%TYPE;
    V_SBLOCKCHALET      ADDRESS_CLIENT.SBLOCKCHALET%TYPE;
    V_STI_INTERIOR      ADDRESS_CLIENT.STI_INTERIOR%TYPE;
    V_SNUM_INTERIOR     ADDRESS_CLIENT.SNUM_INTERIOR%TYPE;
    V_STI_CJHT          ADDRESS_CLIENT.STI_CJHT%TYPE;
    V_SNOM_CJHT         ADDRESS_CLIENT.SNOM_CJHT%TYPE;
    V_SETAPA            ADDRESS_CLIENT.SETAPA%TYPE;
    V_SMANZANA          ADDRESS_CLIENT.SMANZANA%TYPE;
    V_SLOTE             ADDRESS_CLIENT.SLOTE%TYPE;
    V_SREFERENCIA       ADDRESS_CLIENT.SREFERENCIA%TYPE;
    V_NMUNICIPALITY_ORD ADDRESS.NMUNICIPALITY%TYPE;
    V_NPROVINCE_ORD     PROVINCE.NPROVINCE%TYPE;
    V_NLOCAL_ORD        TAB_LOCAT.NLOCAL%TYPE;
    V_SCLIENAME_ORD     CLIENT.SCLIENAME%TYPE;
    V_SIDDOC_ORD        CLIENT_IDDOC.SIDDOC%TYPE;
    V_OFICINA           VARCHAR2(4);
    V_OPE_UBIGEO        VARCHAR2(6);
    V_ORD_RELACION      VARCHAR2(1);
    V_ORD_CONDICION     VARCHAR2(1);
    V_ORD_TIPPER        VARCHAR2(1);
    V_ORD_PAIS          VARCHAR2(2);
    V_BEN_RELACION      VARCHAR2(1);
    V_BEN_CONDICION     VARCHAR2(1);
    V_BEN_PAIS          VARCHAR2(2);
    V_DAT_TIPFON        VARCHAR2(1);
    V_DAT_TIPOPE        VARCHAR2(2);
    V_DAT_ALCANCE       VARCHAR2(1);
    V_DAT_FORMA         VARCHAR2(1);
    V_DISTRITO_ORD      MUNICIPALITY.SDESCRIPT%TYPE;
    V_PROVINCIA_ORD     TAB_LOCAT.SDESCRIPT%TYPE;
    V_DEPARTAMENTO_ORD  PROVINCE.SDESCRIPT%TYPE;
    V_ORD_TIPDOC        VARCHAR2(2);
   
   CURSOR LISTA_SINIESTROS IS
   SELECT * 
      FROM (
             SELECT ROWNUM AS FILA,
             DECODE(TRIM(A.NCLAIM), '', ' ', TRIM(A.NCLAIM)) AS INTERNO,
                    'U' AS MODALIDAD,
                        B.SCLIENT AS BENEFICIARIO,
                        A.NPOLICY,
                        A.NPRODUCT,
                        D.SDESCRIPT,
                        B.DLEDGER_DAT,
                        A.NCERTIF,
                        A.NCLAIM,
                        E.NAMOUNT,
                        B.NCURRENCYPAY,
                        P_TC AS TIPO_CAMBIO,
                        TO_CHAR(E.SOLES) AS SOLES,
                        TO_CHAR(E.DOLARES) AS DOLARES,
                        B.DCOMPDATE,
                        (SELECT DISTINCT SCLIENT
                           FROM CL_COVER
                          WHERE NCLAIM = A.NCLAIM) AS ASEGURADO,
                        RPAD(NVL(C.SDESCRIPT, ' '), 40) AS TIPOPAGO,
                        TRIM(TO_CHAR(CAST((E.NAMOUNT) AS NUMBER(15, 2)), '000000000000000.00')) AS DAT_MTOOPE,
                  DECODE(B.DLEDGER_DAT, '', ' ', TRIM(TO_CHAR(B.DLEDGER_DAT, 'YYYYMMDD'))) AS FECHA,
                  DECODE(B.DCOMPDATE, '', ' ', TRIM(TO_CHAR(B.DCOMPDATE, 'HH24MISS'))) AS HORA
                  FROM CLAIM A
                 INNER JOIN CHEQUES B
                    ON A.NCLAIM = B.NCLAIM
                 INNER JOIN PRODMASTER D
                    ON A.NPRODUCT = D.NPRODUCT
                    AND B.NBRANCH = D.NBRANCH
                  LEFT JOIN TABLE193 C
                    ON C.SREQUEST_TY = B.SREQUEST_TY
                 INNER JOIN (SELECT *
                               FROM (SELECT A.NPOLICY,
                                            A.NPRODUCT,
                                            A.NCERTIF,
                                            A.NCLAIM,
                                            SUM(B.NAMOUNT) AS NAMOUNT,
                                            B.NCURRENCYPAY,
                                            TO_CHAR(SUM(DECODE(B.NCURRENCYPAY, 1, B.NAMOUNT, 0))) AS SOLES,
                                            TO_CHAR(SUM(DECODE(B.NCURRENCYPAY, 2, B.NAMOUNT, ROUND(B.NAMOUNT / P_TC, 4)))) AS DOLARES,
                                            (SELECT DISTINCT SCLIENT
                                               FROM CL_COVER
                                              WHERE NCLAIM = A.NCLAIM) AS ASEGURADO
                                       FROM CLAIM A
                                      INNER JOIN CHEQUES B
                                         ON A.NCLAIM = B.NCLAIM
                                      WHERE NOT B.NCLAIM IS NULL
                                        AND TRUNC(B.DLEDGER_DAT) BETWEEN P_FECINI AND P_FECFIN
                                      GROUP BY A.NPOLICY,
                                               A.NPRODUCT,
                                               A.NCERTIF,
                                               A.NCLAIM,
                                               B.NCURRENCYPAY)
                              WHERE DOLARES > P_MONTO) E
                    ON E.NPOLICY = A.NPOLICY
                   AND E.NPRODUCT = A.NPRODUCT
                   AND E.NCERTIF = A.NCERTIF
                   AND E.NCLAIM = A.NCLAIM
                 WHERE (P_OPE = 1 OR P_OPE = 3)
                   AND TRUNC(B.DLEDGER_DAT) BETWEEN P_FECINI AND P_FECFIN

                UNION ALL

                SELECT *
                  FROM (SELECT ROWNUM AS FILA,
                               DECODE(TRIM(A.NCLAIM), '', ' ', TRIM(A.NCLAIM)) AS INTERNO,
                               'M' AS MODALIDAD,
                                B.SCLIENT AS BENEFICIARIO,
                                A.NPOLICY,
                                A.NPRODUCT,
                                D.SDESCRIPT,
                                B.DLEDGER_DAT,
                                A.NCERTIF,
                                A.NCLAIM,
                                B.NAMOUNT,
                                B.NCURRENCYPAY,
                                P_TC AS TIPO_CAMBIO,
                                TO_CHAR(DECODE(B.NCURRENCYPAY, 1, B.NAMOUNT, 0)) AS SOLES,
                                TO_CHAR(DECODE(B.NCURRENCYPAY, 2, B.NAMOUNT, ROUND(B.NAMOUNT / P_TC, 4))) AS DOLARES,
                                B.DCOMPDATE,
                                (SELECT DISTINCT SCLIENT
                                   FROM CL_COVER
                                  WHERE NCLAIM = A.NCLAIM) AS ASEGURADO,
                                NVL(C.SDESCRIPT, '') AS TIPOPAGO,
                                TRIM(TO_CHAR(CAST((B.NAMOUNT) AS NUMBER(15, 2)), '000000000000000.00')) AS DAT_MTOOPE,
                                DECODE(B.DLEDGER_DAT, '', ' ', TRIM(TO_CHAR(B.DLEDGER_DAT, 'YYYYMMDD'))) AS FECHA,
                                DECODE(B.DCOMPDATE, '', ' ', TRIM(TO_CHAR(B.DCOMPDATE, 'HH24MISS'))) AS HORA
                           FROM CLAIM A
                          INNER JOIN CHEQUES B
                             ON A.NCLAIM = B.NCLAIM
                          INNER JOIN PRODMASTER D
                             ON A.NPRODUCT = D.NPRODUCT
                             AND B.NBRANCH = D.NBRANCH
                           LEFT JOIN TABLE193 C
                             ON C.SREQUEST_TY = B.SREQUEST_TY
                          INNER JOIN CLIENT CL
                             ON CL.SCLIENT = B.SCLIENT
                          WHERE TRUNC(B.DLEDGER_DAT) BETWEEN P_FECINI AND P_FECFIN
                            AND NOT B.NCLAIM IS NULL) Z
                 WHERE Z.BENEFICIARIO IN ((SELECT X.SCLIENT
                                            FROM (SELECT A.NPOLICY,
                                                         A.NPRODUCT,
                                                         D.SDESCRIPT,
                                                         B.DLEDGER_DAT,
                                                         A.NCERTIF,
                                                         A.NCLAIM,
                                                         B.SCLIENT,
                                                         B.NAMOUNT,
                                                         B.NCURRENCYPAY,
                                                         P_TC AS TIPO_CAMBIO,
                                                         TO_CHAR(DECODE(B.NCURRENCYPAY, 1, B.NAMOUNT, ROUND(B.NAMOUNT / P_TC, 4))) AS SOLES,
                                                         TO_CHAR(DECODE(B.NCURRENCYPAY, 2, B.NAMOUNT, ROUND(B.NAMOUNT / P_TC, 4))) AS DOLARES
                                                    FROM CLAIM A
                                                   INNER JOIN CHEQUES B
                                                      ON A.NCLAIM = B.NCLAIM
                                                   INNER JOIN PRODMASTER D
                                                      ON A.NPRODUCT = D.NPRODUCT
                                                   WHERE TRUNC(B.DLEDGER_DAT) BETWEEN P_FECINI AND P_FECFIN
                                                     AND NOT B.NCLAIM IS NULL) X
                                           GROUP BY X.SCLIENT
                                          HAVING SUM(X.DOLARES) > P_MONTO AND COUNT(*) > 1))
                   AND (P_OPE = 2 OR P_OPE = 3)
                   AND Z.DOLARES >= 1000
                --order by 1

                ) K

          LEFT OUTER JOIN (SELECT
                           \*+INDEX(EQUI SYS_C00167813) INDEX(c XPKPROVINCE) INDEX(d XPKTAB_LOCAT) INDEX(e XPKMUNICIPALITY) INDEX(cc CLIENT_COMPLEMENT_PK)*\
                            A.NPERSON_TYP,
                            A.SCLIENAME,
                            A.SFIRSTNAME,
                            A.SLASTNAME,
                            A.SLASTNAME2,
                            A.NQ_CHILD,
                            INSUDB.PKG_REPORTES_TABLERO_CONTROL.
                            FN_DIRE_CORREO_CLIENTE(P_SCLIENT => A.SCLIENT,
                                                   P_NRECOWNER => 2,
                                                   P_STIPO_DATO => 'D',
                                                   P_SIND_INDIVIDUAL => 'N') AS SSTREET,
                            C.SDESCRIPT AS DEPART,
                            D.SDESCRIPT AS PROV,
                            E.SDESCRIPT AS DISTRITO,
                            PH.SPHONE,
                            A.STAX_CODE,
                            E.NMUNICIPALITY,
                            A.SCLIENT,
                            EQUI.COD_UBI_CLI,
                            DCLI.NTYPCLIENTDOC,
                            DCLI.SCLINUMDOCU,
                            DCLI.SCLINUMDOCU AS NUM_DOC_BEN,
                            A.NPERSON_TYP AS TIP_PER,
                            A.NNATIONALITY,
                            NI.SCOD_COUNTRY_ISO,
                            CASE
                              WHEN SUBSTR(A.SCLIENT, 1, 2) = '01' THEN
                               CASE
                                 WHEN SUBSTR(A.SCLIENT, 4, 2) = '20' THEN
                                  '3' --'Persona juridica'
                                 ELSE
                                  '1' --'persona natural con negocio'
                               END
                              ELSE
                               '1' --'Persona natural'
                            END AS TIPO_PERSONA_BEN,
                            CASE
                              WHEN DCLI.NTYPCLIENTDOC = 2 THEN
                               '1'
                              WHEN DCLI.NTYPCLIENTDOC = 4 THEN
                               '2'
                              WHEN DCLI.NTYPCLIENTDOC = 6 THEN
                               '5'
                              WHEN DCLI.NTYPCLIENTDOC = 1 THEN
                               ' '
                              WHEN DCLI.NTYPCLIENTDOC = 3 OR DCLI.NTYPCLIENTDOC = 5 OR DCLI.NTYPCLIENTDOC = 7 OR DCLI.NTYPCLIENTDOC = 8 OR DCLI.NTYPCLIENTDOC = 9 OR DCLI.NTYPCLIENTDOC = 10 OR DCLI.NTYPCLIENTDOC = 11 OR
                                   DCLI.NTYPCLIENTDOC = 0 OR DCLI.NTYPCLIENTDOC = 12 OR DCLI.NTYPCLIENTDOC = 13 THEN
                               '9'
                              ELSE
                               ' '
                            END AS TIPO_DOCUMENTO_BEN,
                            COS.NIDOCUP_SBS AS COD_ESPECIALIDAD_SBS_BEN,
                            DECODE((SELECT MAX(NROLE) FROM ROLES RO
                            WHERE RO.SCLIENT = V_SCLIENT_ORD),1,1,2) AS CLI_HABITUAL
                             FROM CLIENT A

                             LEFT OUTER JOIN (SELECT SCLIENT,
                                                    NLOCAL,
                                                    NPROVINCE,
                                                    NMUNICIPALITY,
                                                    NCOUNTRY,
                                                    SKEYADDRESS,
                                                    SSTREET
                                               FROM ADDRESS ADRR
                                              WHERE ADRR.NRECOWNER = 2
                                                AND ADRR.SRECTYPE = 2
                                                AND ADRR.DNULLDATE IS NULL
                                                AND TRIM(ADRR.SKEYADDRESS) || TO_CHAR(ADRR.DCOMPDATE, 'YYYYMMDDHH24MMSS') || TO_CHAR(ADRR.DEFFECDATE, 'YYYYMMDDHH24MMSS') =
                                                    (SELECT \*+INDEX (AT XIF2110ADDRESS)*\
                                                      MAX(TRIM(AT.SKEYADDRESS) || TO_CHAR(AT.DCOMPDATE, 'YYYYMMDDHH24MMSS') || TO_CHAR(AT.DEFFECDATE, 'YYYYMMDDHH24MMSS'))
                                                       FROM ADDRESS AT
                                                      WHERE AT.SCLIENT = ADRR.SCLIENT
                                                        AND AT.NRECOWNER = 2
                                                        AND AT.SRECTYPE = 2
                                                        AND AT.DNULLDATE IS NULL)

                                             ) ADR
                               ON ADR.SCLIENT = A.SCLIENT

                             LEFT OUTER JOIN (SELECT
                                             \*+INDEX (PHR IDX_PHONE_1)*\
                                              SPHONE,
                                              NKEYPHONES,
                                              SKEYADDRESS,
                                              DCOMPDATE,
                                              DEFFECDATE
                                               FROM PHONES PHR
                                              WHERE PHR.NRECOWNER = 2
                                                AND PHR.DNULLDATE IS NULL
                                                AND TRIM(PHR.SKEYADDRESS) || PHR.NKEYPHONES || TO_CHAR(PHR.DCOMPDATE, 'YYYYMMDDHH24MMSS') || TO_CHAR(PHR.DEFFECDATE, 'YYYYMMDDHH24MMSS') =
                                                    (SELECT \*+INDEX (PHT IDX_PHONE_1)*\
                                                      MAX(TRIM(PHT.SKEYADDRESS) || PHT.NKEYPHONES || TO_CHAR(PHT.DCOMPDATE, 'YYYYMMDDHH24MMSS') || TO_CHAR(PHT.DEFFECDATE, 'YYYYMMDDHH24MMSS'))
                                                       FROM PHONES PHT
                                                      WHERE SUBSTR(PHT.SKEYADDRESS, 2, 14) = SUBSTR(PHR.SKEYADDRESS, 2, 14)
                                                        AND PHT.NRECOWNER = 2
                                                        AND PHT.DNULLDATE IS NULL)

                                             ) PH
                           --ON PH.SKEYADDRESS = ADR.SKEYADDRESS
                               ON SUBSTR(PH.SKEYADDRESS, 2, 14) = A.SCLIENT

                             LEFT JOIN PROVINCE C
                               ON C.NPROVINCE = ADR.NPROVINCE
                             LEFT JOIN TAB_LOCAT D
                               ON D.NLOCAL = ADR.NLOCAL
                             LEFT JOIN MUNICIPALITY E
                               ON E.NMUNICIPALITY = ADR.NMUNICIPALITY
                             LEFT OUTER JOIN EQUI_UBIGEO EQUI
                               ON TO_NUMBER(EQUI.COD_UBI_DIS) = E.NMUNICIPALITY
                              AND EQUI.COD_CLI = '11111111111111'
                             LEFT OUTER JOIN CLIDOCUMENTS DCLI
                               ON DCLI.SCLIENT = A.SCLIENT
                             LEFT OUTER JOIN TBL_TRX_NATIONALITY_ISO NI
                               ON NVL(A.NNATIONALITY, ADR.NCOUNTRY) = NI.NNATIONALITY
                              AND NI.SACTIVE = 1
                             LEFT OUTER JOIN TBL_CONFIG_OCUP_SBS COS
                               ON COS.NIDOCUPACION = A.NSPECIALITY
                              AND COS.SORIGEN_BD = 'TIME'
                              AND COS.SACTIVE = '1') CLBENE
            ON CLBENE.SCLIENT = K.BENEFICIARIO;                               
                               
   
                                  
   BEGIN
     
   \*SINIESTROS_MAYORES_CON(P_OPE => P_OPE,

                               P_TC     => P_TC,
                               P_MONTO  => P_MONTO,
                               P_FECINI => P_FECINI,
                               P_FECFIN => P_FECFIN,
                               C_TABLE  => C_TABLE);*\
     
   EXECUTE IMMEDIATE 'ALTER SESSION SET NLS_DATE_FORMAT = ''DD/MM/YYYY''';
   
   FOR C1 IN LISTA_SINIESTROS
    LOOP
      V_TABLE_LISTA_REPORTE_SBS.EXTEND();
        V_TABLE_LISTA_REPORTE_SBS(V_TABLE_LISTA_REPORTE_SBS.COUNT) := T_RECORD_LISTA_REPORTE_SBS(
        FILA => TRIM(TO_CHAR(C1.FILA, '00000000')),
         OFICINA => '001 ',
         OPERACION => TRIM(TO_CHAR(C1.FILA, '00000000')),
         INTERNO => RPAD(C1.INTERNO, 20),
         MODALIDAD => C1.MODALIDAD,
         OPE_UBIGEO => '150141',
         OPE_FECHA => RPAD(C1.FECHA, 8), --7
         OPE_HORA => RPAD(C1.HORA, 6), --8
         EJE_RELACION => RPAD(' ', 1),
         EJE_CONDICION => RPAD(' ', 1),
         EJE_TIPPER => RPAD(' ', 1),
         EJE_TIPDOC => RPAD(' ', 1),
         EJE_NUMDOC => RPAD(' ', 12),
         EJE_NUMRUC => RPAD(' ', 11),
         EJE_APEPAT => RPAD(' ', 40),
         EJE_APEMAT => RPAD(' ', 40),
         EJE_NOMBRES => RPAD(' ', 40),
         EJE_OCUPACION => RPAD(' ', 4), 
         EJE_PAIS => RPAD(' ', 6),
         EJE_CARGO => RPAD(' ', 104), --vb.847
         EJE_PEP => RPAD(' ', 2),
         EJE_DOMICILIO => RPAD(' ', 150), --vb.848
         EJE_DEPART => RPAD(' ', 2), --vb.849
         EJE_PROV => RPAD(' ', 2), --vb.850
         EJE_DIST => RPAD(' ', 2), --vb.851
         EJE_TELEFONO => RPAD(' ', 40),
         ORD_RELACION => RPAD(V_CLI_HABITUAL, 1),
         ORD_CONDICION => RPAD(NVL(V_ORD_CONDICION, ' '), 1),
         ORD_TIPPER => RPAD(NVL(V_ORD_TIPPER, ' '), 1),
         ORD_TIPDOC => RPAD(CASE
                WHEN V_ORD_TIPPER IN ('1', '2') THEN
                 NVL(V_ORD_TIPDOC, ' ')
                ELSE
                 ' '
              END

             ,
              1), --30-vb.738
         ORD_NUMDOC => RPAD(CASE
                WHEN V_ORD_TIPPER IN ('1', '2') THEN
                 NVL(V_SIDDOC_ORD, ' ')
                ELSE
                 ' '
              END,
              12), --31-vb.671
         ORD_NUMRUC => RPAD(CASE
                WHEN V_ORD_TIPPER IN (3, 4) THEN
                 NVL(V_SIDDOC_ORD, ' ')
                ELSE
                 ' '
              END,
              11),
         ORD_APEPAT => RPAD(NVL(V_SCLIENAME_ORD, ' '), 120), --33
         ORD_APEMAT => RPAD(' ', 40), --34
         ORD_NOMBRES => RPAD(' ', 40), --35
         ORD_OCUPACION => RPAD(' ', 4), --36-vb.744
         ORD_PAIS => RPAD(NVL(V_ORD_PAIS,' '), 6), --37
         ORD_CARGO => RPAD(' ', 104), --38-NO ES OBLIGATORIO.vb747
         ORD_PEP => RPAD(' ', 2), --39-NO ES OBLIGATORIO
         ORD_DOMICILIO => RPAD(UPPER(NVL(TRIM(V_SDIRECCION_ORD||' '),' ')),150), --40
         ORD_DEPART => NULL,--RPAD(DECODE(V_NMUNICIPALITY_ORD, '', '15', SUBSTR(V_NMUNICIPALITY_ORD, 1, 2)), 2), --41
         ORD_PROV => NULL,--RPAD(DECODE(V_NMUNICIPALITY_ORD, '', '01', SUBSTR(V_NMUNICIPALITY_ORD, 3, 2)), 2), --42
         ORD_DIST => NULL,--RPAD(DECODE(V_NMUNICIPALITY_ORD, '', '41', SUBSTR(V_NMUNICIPALITY_ORD, 5, 2)), 2), --43
         ORD_TELEFONO => RPAD(NVL(V_SPHONE_ORD, ' '), 40),
         BEN_RELACION => RPAD(C1.CLI_HABITUAL, 1),
         BEN_CONDICION => RPAD('1', 1),
         BEN_TIP_PER => RPAD(NVL(C1.TIPO_PERSONA_BEN, ' '), 1),
         BEN_TIP_DOC => RPAD(NVL(C1.TIPO_DOCUMENTO_BEN, ' '), 1),
         BEN_NUM_DOC => RPAD(CASE
                WHEN C1.TIPO_PERSONA_BEN IN ('1', '2') THEN
                 NVL(C1.NUM_DOC_BEN, ' ')
                ELSE
                 ' '
              END,
              12), --49
         BEN_NUM_RUC => RPAD(CASE
                WHEN C1.TIPO_PERSONA_BEN IN ('3', '4') THEN
                 RIGHT(C1.SCLIENT, 11)
                ELSE
                 ' '
              END,
              11),
         BEN_APEPAT => RPAD(REPLACE(REPLACE(REPLACE(CASE
                                        WHEN C1.TIPO_PERSONA_BEN IN ('1', '2') THEN
                                         NVL(C1.SLASTNAME, ' ')
                                        WHEN C1.TIPO_PERSONA_BEN IN ('3', '4') THEN
                                         NVL(C1.SCLIENAME, ' ')
                                        ELSE
                                         ' '
                                      END,
                                      '?',
                                      '#'),
                              'Ñ',
                              '#'),
                      'ñ',
                      '#'),
              120),
         BEN_APEMAT => RPAD(REPLACE(REPLACE(REPLACE(NVL(C1.SLASTNAME2, ' '), '?', '#'), 'Ñ', '#'), 'ñ', '#'), 40), --52
         BEN_NOMBRES => RPAD(REPLACE(REPLACE(REPLACE(NVL(C1.SFIRSTNAME, ' '), '?', '#'), 'Ñ', '#'), 'ñ', '#'), 40), --53

         BEN_OCUPACION => RPAD(NVL(C1.COD_ESPECIALIDAD_SBS_BEN, '0999'), 4),
         BEN_PAIS => RPAD('PE', 6),
         BEN_CARGO => RPAD(' ', 104),
         BEN_PEP => RPAD(' ', 2),
         BEN_DOMICILIO => NULL,--RPAD(REPLACE(REPLACE(REPLACE(UPPER(DECODE(CLBENE.SSTREET, '', ' ', TRIM(CLBENE.SSTREET)))
                                     --,'?','#'),'Ñ','#'),'ñ','#'),150),
         BEN_DEPART => NULL,--RPAD(DECODE(C1.NMUNICIPALITY, '', '15', SUBSTR(TRIM(TO_CHAR((C1.COD_UBI_CLI), '000000')), 1, 2)), 2), --59
         BEN_PROV => NULL,--RPAD(DECODE(C1.NMUNICIPALITY, '', '01', SUBSTR(TRIM(TO_CHAR((C1.COD_UBI_CLI), '000000')), 3, 2)), 2),
         BEN_DIST => NULL,--RPAD(DECODE(C1.NMUNICIPALITY, '', '41', SUBSTR(TRIM(TO_CHAR((C1.COD_UBI_CLI), '000000')), 5, 2)), 2),
         BEN_TELEFONO => RPAD(NVL(C1.SPHONE, ' '), 40), --62
         DAT_TIPFON => NULL,--RPAD(DECODE(V_DAT_TIPFON, '', ' ', V_DAT_TIPFON), 1), --63-vb.807
         DAT_TIPOPE => NULL,--RPAD(DECODE(V_DAT_TIPOPE, '', ' ', V_DAT_TIPOPE), 2), --64-vb.808
         DAT_DESOPE => RPAD(' ', 40), --65-vb.809
         DAT_ORIFON => RPAD(' ', 80), --66-vb.810
         DAT_MONOPE => NULL,--RPAD(DECODE(C1.NCURRENCYPAY, '1', 'PEN', 'USD'), 3), --67
         DAT_MONOPE_A => RPAD(' ', 3), --68
         DAT_MTOOPE => RPAD(C1.DAT_MTOOPE, 29),
         DAT_MTOOPEA => RPAD(' ', 30), --MTO_PENSIONGAR , MTO_PRIUNI 70
         DAT_COD_ENT_INVO => RPAD(' ', 5), --71--NO OBLIGATORIO
         DAT_COD_TIP_CTAO => RPAD(' ', 1), --72--NO OBLIGATORIO
         DAT_COD_CTAO => RPAD(' ', 20), --73--NO OBLIGATORIO
         DAT_ENT_FNC_EXTO => RPAD(' ', 150), --74--NO OBLIGATORIO
         DAT_COD_ENT_INVB => RPAD(' ', 5), --75--NO OBLIGATORIO
         DAT_COD_TIP_CTAB => RPAD(' ', 1), --76--NO OBLIGATORIO
         DAT_COD_CTAB => RPAD(' ', 20), --77--NO OBLIGATORIO
         DAT_ENT_FNC_EXTB => RPAD(' ', 150), --78--NO OBLIGATORIO
         --RPAD(DECODE(V_DAT_ALCANCE, '', ' ', V_DAT_ALCANCE), 1) AS DAT_ALCANCE, --79-vb.883
         DAT_COD_PAISO => RPAD(' ', 2), --80--ALCANCE 1, ESTE CAMPO VA EN BLANCO
         DAT_COD_PAISD => RPAD(' ', 2), --81--ALCANCE 1, ESTE CAMPO VA EN BLANCO
         DAT_INTOPE => RPAD('2', 1), --82--NO OBLIGATORIO
         --DAT_FORMA => RPAD(DECODE(V_DAT_FORMA, '', ' ', V_DAT_FORMA), 1), --83-vb.887
         DAT_INFORM => C1.TIPOPAGO--RPAD(DECODE(C1.TIPOPAGO, '', ' ', C1.TIPOPAGO), 40), --84--NO OBLIGATORIO
         );
    
      
    END LOOP;
   
   OPEN C_TABLE FOR

      SELECT * FROM DUAL;
   END;*/
   
   PROCEDURE SINIESTROS_MAYORES_CON(P_OPE    NUMBER,
                               P_TC     NUMBER,
                               P_MONTO  NUMBER,
                               P_FECINI VARCHAR2,
                               P_FECFIN VARCHAR2,
                               C_TABLE  OUT MYCURSOR) IS
                               
                           
   BEGIN
     
     --EXECUTE IMMEDIATE 'ALTER SESSION SET NLS_DATE_FORMAT = ''DD/MM/YYYY''';
   OPEN C_TABLE FOR

      SELECT * 
      FROM (
             SELECT 'U' AS MODALIDAD,
                        B.SCLIENT AS BENEFICIARIO,
                        A.NPOLICY,
                        A.NPRODUCT,
                        D.SDESCRIPT,
                        B.DLEDGER_DAT,
                        A.NCERTIF,
                        A.NCLAIM,
                        E.NAMOUNT,
                        B.NCURRENCYPAY,
                        P_TC AS TIPO_CAMBIO,
                        TO_CHAR(E.SOLES) AS SOLES,
                        TO_CHAR(E.DOLARES) AS DOLARES,
                        B.DCOMPDATE,
                        (SELECT DISTINCT SCLIENT
                           FROM CL_COVER
                          WHERE NCLAIM = A.NCLAIM) AS ASEGURADO,
                        --NVL(C.SDESCRIPT, '') AS TIPOPAGO,
                        DECODE(B.SREQUEST_TY,4,'Medios o plataformas virtual',' ') AS TIPOPAGO,
                        B.NRECEIPT,
                        DECODE(B.SREQUEST_TY,4,'3',' ') AS DAT_FORMA
                  FROM CLAIM A
                 INNER JOIN CHEQUES B
                    ON A.NCLAIM = B.NCLAIM
                 INNER JOIN PRODMASTER D
                    ON A.NPRODUCT = D.NPRODUCT
                    AND B.NBRANCH = D.NBRANCH
                  /*LEFT JOIN TABLE193 C
                    ON C.SREQUEST_TY = B.SREQUEST_TY*/
                 INNER JOIN (SELECT *
                               FROM (SELECT A.NPOLICY,
                                            A.NPRODUCT,
                                            A.NCERTIF,
                                            A.NCLAIM,
                                            SUM(B.NAMOUNT) AS NAMOUNT,
                                            B.NCURRENCYPAY,
                                            TO_CHAR(SUM(DECODE(B.NCURRENCYPAY, 1, B.NAMOUNT, 0))) AS SOLES,
                                            TO_CHAR(SUM(DECODE(B.NCURRENCYPAY, 2, B.NAMOUNT, ROUND(B.NAMOUNT / P_TC, 4)))) AS DOLARES,
                                            (SELECT DISTINCT SCLIENT
                                               FROM CL_COVER
                                              WHERE NCLAIM = A.NCLAIM) AS ASEGURADO
                                       FROM CLAIM A
                                      INNER JOIN CHEQUES B
                                         ON A.NCLAIM = B.NCLAIM
                                      WHERE NOT B.NCLAIM IS NULL
                                        AND TRUNC(B.DLEDGER_DAT) BETWEEN P_FECINI AND P_FECFIN
                                      GROUP BY A.NPOLICY,
                                               A.NPRODUCT,
                                               A.NCERTIF,
                                               A.NCLAIM,
                                               B.NCURRENCYPAY)
                              WHERE DOLARES > P_MONTO) E
                    ON E.NPOLICY = A.NPOLICY
                   AND E.NPRODUCT = A.NPRODUCT
                   AND E.NCERTIF = A.NCERTIF
                   AND E.NCLAIM = A.NCLAIM
                 WHERE (P_OPE = 1 OR P_OPE = 3)
                   AND TRUNC(B.DLEDGER_DAT) BETWEEN P_FECINI AND P_FECFIN

                UNION ALL

                SELECT *
                  FROM (SELECT 'M' AS MODALIDAD,
                                B.SCLIENT AS BENEFICIARIO,
                                A.NPOLICY,
                                A.NPRODUCT,
                                D.SDESCRIPT,
                                B.DLEDGER_DAT,
                                A.NCERTIF,
                                A.NCLAIM,
                                B.NAMOUNT,
                                B.NCURRENCYPAY,
                                P_TC AS TIPO_CAMBIO,
                                TO_CHAR(DECODE(B.NCURRENCYPAY, 1, B.NAMOUNT, 0)) AS SOLES,
                                TO_CHAR(DECODE(B.NCURRENCYPAY, 2, B.NAMOUNT, ROUND(B.NAMOUNT / P_TC, 4))) AS DOLARES,
                                B.DCOMPDATE,
                                (SELECT DISTINCT SCLIENT
                                   FROM CL_COVER
                                  WHERE NCLAIM = A.NCLAIM) AS ASEGURADO,
                                --NVL(C.SDESCRIPT, '') AS TIPOPAGO,
                                DECODE(B.SREQUEST_TY,4,'Medios o plataformas virtual',' ') AS TIPOPAGO,
                                B.NRECEIPT,
                                DECODE(B.SREQUEST_TY,4,'3',' ') AS DAT_FORMA
                           FROM CLAIM A
                          INNER JOIN CHEQUES B
                             ON A.NCLAIM = B.NCLAIM
                          INNER JOIN PRODMASTER D
                             ON A.NPRODUCT = D.NPRODUCT
                             AND B.NBRANCH = D.NBRANCH
                           /*LEFT JOIN TABLE193 C
                             ON C.SREQUEST_TY = B.SREQUEST_TY*/
                          INNER JOIN CLIENT CL
                             ON CL.SCLIENT = B.SCLIENT
                          WHERE TRUNC(B.DLEDGER_DAT) BETWEEN P_FECINI AND P_FECFIN
                            AND NOT B.NCLAIM IS NULL) Z
                 WHERE Z.BENEFICIARIO IN ((SELECT X.SCLIENT
                                            FROM (SELECT A.NPOLICY,
                                                         A.NPRODUCT,
                                                         D.SDESCRIPT,
                                                         B.DLEDGER_DAT,
                                                         A.NCERTIF,
                                                         A.NCLAIM,
                                                         B.SCLIENT,
                                                         B.NAMOUNT,
                                                         B.NCURRENCYPAY,
                                                         P_TC AS TIPO_CAMBIO,
                                                         TO_CHAR(DECODE(B.NCURRENCYPAY, 1, B.NAMOUNT, ROUND(B.NAMOUNT / P_TC, 4))) AS SOLES,
                                                         TO_CHAR(DECODE(B.NCURRENCYPAY, 2, B.NAMOUNT, ROUND(B.NAMOUNT / P_TC, 4))) AS DOLARES
                                                    FROM CLAIM A
                                                   INNER JOIN CHEQUES B
                                                      ON A.NCLAIM = B.NCLAIM
                                                   INNER JOIN PRODMASTER D
                                                      ON A.NPRODUCT = D.NPRODUCT
                                                   WHERE TRUNC(B.DLEDGER_DAT) BETWEEN P_FECINI AND P_FECFIN
                                                     AND NOT B.NCLAIM IS NULL) X
                                           GROUP BY X.SCLIENT
                                          HAVING SUM(X.DOLARES) > P_MONTO AND COUNT(*) > 1))
                   AND (P_OPE = 2 OR P_OPE = 3)
                   AND Z.DOLARES >= 1000
                --order by 1

                ) K

          LEFT OUTER JOIN (SELECT
                           /*+INDEX(EQUI SYS_C00167813) INDEX(c XPKPROVINCE) INDEX(d XPKTAB_LOCAT) INDEX(e XPKMUNICIPALITY) INDEX(cc CLIENT_COMPLEMENT_PK)*/
                            A.NPERSON_TYP,
                            A.SCLIENAME,
                            A.SFIRSTNAME,
                            A.SLASTNAME,
                            A.SLASTNAME2,
                            A.NQ_CHILD,
                            INSUDB.PKG_REPORTES_TABLERO_CONTROL.
                            FN_DIRE_CORREO_CLIENTE(P_SCLIENT => A.SCLIENT,
                                                   P_NRECOWNER => 2,
                                                   P_STIPO_DATO => 'D',
                                                   P_SIND_INDIVIDUAL => 'N') AS SSTREET,
                            C.SDESCRIPT AS DEPART,
                            D.SDESCRIPT AS PROV,
                            E.SDESCRIPT AS DISTRITO,
                            PH.SPHONE,
                            A.STAX_CODE,
                            E.NMUNICIPALITY,
                            A.SCLIENT,
                            EQUI.COD_UBI_CLI,
                            DCLI.NTYPCLIENTDOC,
                            DCLI.SCLINUMDOCU,
                            DCLI.SCLINUMDOCU AS NUM_DOC_BEN,
                            A.NPERSON_TYP AS TIP_PER,
                            A.NNATIONALITY,
                            NI.SCOD_COUNTRY_ISO,
                            CASE
                              WHEN SUBSTR(A.SCLIENT, 1, 2) = '01' THEN
                               CASE
                                 WHEN SUBSTR(A.SCLIENT, 4, 2) = '20' THEN
                                  '3' --'Persona juridica'
                                 ELSE
                                  '1' --'persona natural con negocio'
                               END
                              ELSE
                               '1' --'Persona natural'
                            END AS TIPO_PERSONA_BEN,
                            CASE
                              WHEN DCLI.NTYPCLIENTDOC = 2 THEN
                               '1'
                              WHEN DCLI.NTYPCLIENTDOC = 4 THEN
                               '2'
                              WHEN DCLI.NTYPCLIENTDOC = 6 THEN
                               '5'
                              WHEN DCLI.NTYPCLIENTDOC = 1 THEN
                               ' '
                              WHEN DCLI.NTYPCLIENTDOC = 3 OR DCLI.NTYPCLIENTDOC = 5 OR DCLI.NTYPCLIENTDOC = 7 OR DCLI.NTYPCLIENTDOC = 8 OR DCLI.NTYPCLIENTDOC = 9 OR DCLI.NTYPCLIENTDOC = 10 OR DCLI.NTYPCLIENTDOC = 11 OR
                                   DCLI.NTYPCLIENTDOC = 0 OR DCLI.NTYPCLIENTDOC = 12 OR DCLI.NTYPCLIENTDOC = 13 THEN
                               '9'
                              ELSE
                               ' '
                            END AS TIPO_DOCUMENTO_BEN,
                            COS.NIDOCUP_SBS AS COD_ESPECIALIDAD_SBS_BEN
                             FROM CLIENT A

                             LEFT OUTER JOIN (SELECT SCLIENT,
                                                    NLOCAL,
                                                    NPROVINCE,
                                                    NMUNICIPALITY,
                                                    NCOUNTRY,
                                                    SKEYADDRESS,
                                                    SSTREET
                                               FROM ADDRESS ADRR
                                              WHERE ADRR.NRECOWNER = 2
                                                AND ADRR.SRECTYPE = 2
                                                AND ADRR.DNULLDATE IS NULL
                                                AND TRIM(ADRR.SKEYADDRESS) || TO_CHAR(ADRR.DCOMPDATE, 'YYYYMMDDHH24MMSS') || TO_CHAR(ADRR.DEFFECDATE, 'YYYYMMDDHH24MMSS') =
                                                    (SELECT /*+INDEX (AT XIF2110ADDRESS)*/
                                                      MAX(TRIM(AT.SKEYADDRESS) || TO_CHAR(AT.DCOMPDATE, 'YYYYMMDDHH24MMSS') || TO_CHAR(AT.DEFFECDATE, 'YYYYMMDDHH24MMSS'))
                                                       FROM ADDRESS AT
                                                      WHERE AT.SCLIENT = ADRR.SCLIENT
                                                        AND AT.NRECOWNER = 2
                                                        AND AT.SRECTYPE = 2
                                                        AND AT.DNULLDATE IS NULL)

                                             ) ADR
                               ON ADR.SCLIENT = A.SCLIENT

                             LEFT OUTER JOIN (SELECT
                                             /*+INDEX (PHR IDX_PHONE_1)*/
                                              SPHONE,
                                              NKEYPHONES,
                                              SKEYADDRESS,
                                              DCOMPDATE,
                                              DEFFECDATE
                                               FROM PHONES PHR
                                              WHERE PHR.NRECOWNER = 2
                                                AND PHR.DNULLDATE IS NULL
                                                AND TRIM(PHR.SKEYADDRESS) || PHR.NKEYPHONES || TO_CHAR(PHR.DCOMPDATE, 'YYYYMMDDHH24MMSS') || TO_CHAR(PHR.DEFFECDATE, 'YYYYMMDDHH24MMSS') =
                                                    (SELECT /*+INDEX (PHT IDX_PHONE_1)*/
                                                      MAX(TRIM(PHT.SKEYADDRESS) || PHT.NKEYPHONES || TO_CHAR(PHT.DCOMPDATE, 'YYYYMMDDHH24MMSS') || TO_CHAR(PHT.DEFFECDATE, 'YYYYMMDDHH24MMSS'))
                                                       FROM PHONES PHT
                                                      WHERE SUBSTR(PHT.SKEYADDRESS, 2, 14) = SUBSTR(PHR.SKEYADDRESS, 2, 14)
                                                        AND PHT.NRECOWNER = 2
                                                        AND PHT.DNULLDATE IS NULL)

                                             ) PH
                           --ON PH.SKEYADDRESS = ADR.SKEYADDRESS
                               ON SUBSTR(PH.SKEYADDRESS, 2, 14) = A.SCLIENT

                             LEFT JOIN PROVINCE C
                               ON C.NPROVINCE = ADR.NPROVINCE
                             LEFT JOIN TAB_LOCAT D
                               ON D.NLOCAL = ADR.NLOCAL
                             LEFT JOIN MUNICIPALITY E
                               ON E.NMUNICIPALITY = ADR.NMUNICIPALITY
                             LEFT OUTER JOIN EQUI_UBIGEO EQUI
                               ON TO_NUMBER(EQUI.COD_UBI_DIS) = E.NMUNICIPALITY
                              AND EQUI.COD_CLI = '11111111111111'
                             LEFT OUTER JOIN CLIDOCUMENTS DCLI
                               ON DCLI.SCLIENT = A.SCLIENT
                             LEFT OUTER JOIN TBL_TRX_NATIONALITY_ISO NI
                               ON NVL(A.NNATIONALITY, ADR.NCOUNTRY) = NI.NNATIONALITY
                              AND NI.SACTIVE = 1
                             LEFT OUTER JOIN TBL_CONFIG_OCUP_SBS COS
                               ON COS.NIDOCUPACION = A.NSPECIALITY
                              AND COS.SORIGEN_BD = 'TIME'
                              AND COS.SACTIVE = '1') CLBENE
            ON CLBENE.SCLIENT = K.BENEFICIARIO;
   END;

END;
/
