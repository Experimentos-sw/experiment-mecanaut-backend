Feature: Creación de un plan de mantenimiento dinámico
  Como administrador
  Quiero crear un plan de mantenimiento para una línea de producción y agregarlo al calendario con una duración
  Para organizar a mis técnicos

  Scenario: Agregación exitosa de plan de mantenimiento
    Given que el administrador está autenticado
    When llena los campos en el formulario de plan de mantenimiento con límite de métrica
    Then el plan de mantenimiento se agrega al calendario exitosamente

  Scenario: Falla en la agregación por campos requeridos faltantes
    Given que el administrador está autenticado
    When envía el formulario de plan de mantenimiento sin llenar todos los campos
    Then el sistema solicita completar los campos requeridos
