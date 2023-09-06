function manejarClickAgregarPaso() {
    editarTareaVM.pasos.push(new pasoViewModel({ modoEdicion: true, realizado: false }));
    $("[name=txtPasoDescripcion]:visible").focus();
}