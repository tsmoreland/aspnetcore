async function setImageUsingStreaming(elementId, imageStream) {
    const arrayBuffer = await imageStream.arrayBuffer();
    const blob = new Blob([arrayBuffer]);
    const url = URL.createObjectURL(blob);

    const element = document.getElementById(elementId);
    if (element !== undefined && element !== null) {
        element.style.backgroundImage = `url(${url})`
        element.style.display = 'block'
        element.style.backgroundSize = 'contain';
        element.style.backgroundRepeat = 'no-repeat';
        element.style.backgroundPosition = 'center center';
        element.style.height = 'auto';
        element.style.width = 'auto';
        element.style.maxHeight = '100%';
        element.style.maxWidth = '100%';
        element.style.flex = '1 1 auto';
    } else {
        console.log(imageElementId + ' not found')
    }
} 
