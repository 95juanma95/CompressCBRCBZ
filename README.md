
# Compresor y Transformador de CBR/CBZ

Aplicaci�n de consola para la transformaci�n de archivos CBR a CBZ, y para, en caso que se necesite, se puedan comprimir las imagenes que componen el archivo de salida para que ocupen menos espacio.


## Utilizaci�n

Abrir cmd, modificar el directorio para que sea la carpeta donde se ha depositado la aplicaci�n.

Al ejecutar la aplicacion CompressCBRCBZ.exe, se puede ejecutar con el comando -help para obtener ayuda sobre que comandos recibe y como usarlos.

```bash
  CompressCBRCBZ.exe -help
```
Los argumentos deben ser los siguientes, separados por un espacio en blanco:
- `"-input"`: `Obligatorio` Archivo de entrada en formato cbz o cbr.
  - Ejemplo: "-input:C:\Comics\Prueba.cbr"
- `"-output"`: `Opcional` Archivo de salida en formato cbz.
  - Ejemplo: "-output:C:\Comics\Prueba.cbz"
- `"-calidad"`: `Opcional` Calidad de la transformaci�n, escala de 0 a 100, donde 0 es calidad minima y 100 es mantener toda la calidad.
  - Ejemplo: "-calidad:25"
- `"-PPI"`: `Opcional` Cantidad de Pixel Per Inch de la imagen a transformar.
  - Ejemplo: "-PPI:48"

Ejemplo de uso
```bash
  CompressCBRCBZ.exe "-input:C:\Comics\Prueba.cbr" "-output:C:\Comics\Prueba.cbz" "-calidad:25" "-PPI:48"
```


## Licencia

[MIT](https://choosealicense.com/licenses/mit/)
