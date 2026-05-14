Feature: Alerta por métrica crítica
  Como administrador
  Quiero simular el registro de métricas de una maquinaria
  Para generar alertas si supera los umbrales seguros

  Scenario: Alerta por métrica crítica que supera límite
    Given que una máquina tiene una temperatura normal
    When se registra una lectura de temperatura que supera el límite de seguridad
    Then el estado de la máquina cambia a advertencia o mantenimiento requerido

  Scenario: Métrica normal no cambia el estado
    Given que una máquina tiene una temperatura normal
    When se actualiza una metrica con valores seguros
    Then el sistema actualiza la metrica y la máquina sigue operativa
