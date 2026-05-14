Feature: Asignación de técnico a orden de trabajo
  Como administrador
  Quiero asignar técnicos a órdenes de trabajo
  Para asegurar su ejecución

  Scenario: Asignación exitosa sin conflictos
    Given que existe una orden abierta
    When la administradora asigna a un técnico disponible
    Then el sistema confirma la asignación y actualiza la orden

  Scenario: Técnicos no disponibles o conflicto de asignación
    Given que existe una orden abierta
    When la administradora intenta asignarle una nueva a un técnico que ya tiene tareas
    Then el sistema alerta sobre la carga de trabajo o disponibilidad