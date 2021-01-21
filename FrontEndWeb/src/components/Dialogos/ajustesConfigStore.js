import {localStore} from '../../localStore'
let configInicial = {
    fondoAburrido: false,
    colorFondo: "#101923",
    usarImagen: false,
    imagen:"/imagenes/rosed.png",
    modoCover: true,
    scrollAncho: false,
    tagClasico: false,
    palabrasHideadas: ""
}

// Cargo configuracion gudardada
export default localStore('ajustes_config', configInicial)