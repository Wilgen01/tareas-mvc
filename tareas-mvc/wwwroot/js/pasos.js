function manejarClickAgregarPaso() {
    editarTareaVM.pasos.push(new pasoViewModel({ modoEdicion: true, realizado: false }));
    $("[name=txtPasoDescripcion]:visible").focus();
}

function manejarClickCancelarPaso(paso) {
    if (paso.esNuevo()) {
        editarTareaVM.pasos.pop();
    }
}

async function manejarClickSalvarPaso(paso) {
    paso.modoEdicion(false)
    const esNuevo = paso.esNuevo();
    const idTarea = editarTareaVM.id;

    const data = obtenerCuerpoPeticionPaso(paso);

    if (esNuevo) {

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