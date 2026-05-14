Feature: Ejecución de orden y descuento de stock
  Como técnico
  Quiero marcar las tareas que ya he realizado
  Para mantener un seguimiento claro de mi trabajo y actualizar el inventario usado

  Scenario: Ejecución exitosa de orden de trabajo y descuento de stock
    Given que un técnico tiene una orden asignada y el inventario del "Filtro" es 10
    When el técnico completa la orden indicando que usó 2 "Filtros"
    Then la orden cambia a Completada y el inventario se reduce a 8

  Scenario: Fallo al guardar tarea completada con cantidad de stock inválida
    Given que un técnico tiene una orden asignada y el inventario del "Filtro" es 10
    When el técnico intenta registrar una cantidad inválida de 15 "Filtros" usados
    Then el sistema muestra un mensaje indicando que el valor ingresado no es válido
