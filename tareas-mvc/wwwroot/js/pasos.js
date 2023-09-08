function manejarClickAgregarPaso() {
    editarTareaVM.pasos.push(new pasoViewModel({ modoEdicion: true, realizado: false }));
    $("[name=txtPasoDescripcion]:visible").focus();
}

function manejarClickCancelarPaso(paso) {
    if (paso.esNuevo()) {
        editarTareaVM.pasos.pop();
    } else {
        paso.descripcion(paso.descripcionAnterior)
        paso.modoEdicion(false)
    }
}

async function manejarClickSalvarPaso(paso) {
    paso.modoEdicion(false)
    const esNuevo = paso.esNuevo();
    const idTarea = editarTareaVM.id;

    const data = obtenerCuerpoPeticionPaso(paso);

    if (esNuevo) {
        await insertarPaso(paso, data, idTarea)
    }
}

async function insertarPaso(paso, data, idTarea) {
    const respuesta = await fetch(`${urlPasos}/${idTarea}`, {
        body: data,
        method: "POST",
        headers: {
            'Content-Type': 'application/json'
        }
    })

    if (respuesta.ok) {
        const json = await respuesta.json();
        paso.id(json.id)
    }
}

function obtenerCuerpoPeticionPaso(paso) {
    return JSON.stringify({
        descripcion: paso.descripcion(),
        realizado: paso.realizado()
    })
}

function manejarClickDescripcionPaso(paso) {
    paso.modoEdicion(true);
    paso.descripcionAnterior = paso.descripcion()
    $("[name=txtPasoDescripcion]:visible").focus();
}