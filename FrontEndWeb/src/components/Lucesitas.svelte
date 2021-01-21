
<script>
    let luces = []

    for (let i = 0; i < 50; i++) {
        luces.push({color:colorRandom(), opacity: 1})
    }
    luces = luces

    setInterval(() => {
        luces = luces.map(l => {
            if(Math.random() < 0.1) {
                l.opacity = Math.random()
            }
            return l
        })
        luces = luces
    }, 200);

    console.log(luces)
    function colorRandom() {
        let colores = ['255,253,65','126,255,76', '153,250,255', '255,136,241' ]
        return colores[Math.floor(Math.random()* colores.length)]
    }
</script>

<div class="lucesitas">
    {#each luces as luz}
        <div 
            class="luz" 
            style={`transform:translateX(${0}px) translateY(${0}px);
            background: red;
            opacity: ${luz.opacity}
            `
        }
></div>
    {/each}
</div>
<style>
    .lucesitas {
        display:flex;
        justify-content: space-between;
        position: absolute;
        width: 100%;
    }

    .luz {
        height:20px;
        width:20px;
        /* background:white;
        box-shadow: aliceblue 0 0 20px; */
        background: radial-gradient(circle, rgba(255,253,65,1) 0%,rgba(255,253,65,0.9) 15%, rgba(255,253,65,0.5) 20%, rgba(9, 9, 121, 0) 50%);
        transition: 1s;
    }
</style>