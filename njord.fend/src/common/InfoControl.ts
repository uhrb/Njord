import * as L from 'leaflet';

// Создаем информационную панель
export class InfoControl<T> extends L.Control {
    private _div!: HTMLElement;
    private _renderer: (item?: T) => string;

    constructor(renderer: (item?: T) => string, props: any) {
        super(props);
        this._renderer = renderer;
    }

    onAdd(map: L.Map) {
        this._div = L.DomUtil.create('div', 'info');
        this.update();
        return this._div;
    }

    update(stats?: T) {
        this._div.innerHTML = this._renderer(stats);
    }
}
