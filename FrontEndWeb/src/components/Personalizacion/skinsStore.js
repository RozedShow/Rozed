import {localStore} from '../../localStore'
import skinsPorDefecto from './skinsPorDefecto'

let store = localStore("skins", {
    activo:'Classic',
    skins: skinsPorDefecto
})

store.applicarEstilo = function aplicarEstilo() {
    store.update(skins => {
        document.getElementById("skin")
            .innerHTML = skins.skins.filter(s => s.nombre == skins.activo)[0].style
        return skins;
    })
}
store.restaurarSkins = function restaurarSkins() {
    store.update(skins => {
        skins =  {
            activo:'Classic',
            skins: skinsPorDefecto
        }
        console.log(skins)
        document.getElementById("skin")
            .innerHTML = skins.skins.filter(s => s.nombre == skins.activo)[0].style
        return skins;
    })
}
export default store