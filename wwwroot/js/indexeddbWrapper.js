var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
let dotnetReference;
let database;
export function initializeDatabase(reference, dbName) {
    dotnetReference = reference;
    const openRequest = window.indexedDB.open(dbName, 4);
    openRequest.onsuccess = () => __awaiter(this, void 0, void 0, function* () {
        console.log("initialized database " + dbName);
        database = openRequest.result;
        yield dotnetReference.invokeMethodAsync("DatabaseInitialized");
    });
    openRequest.onupgradeneeded = () => {
        database = openRequest.result;
        database.createObjectStore("pairs", {
            keyPath: "id",
            autoIncrement: true,
        });
    };
    openRequest.onerror = (event) => {
        console.error(`cannot open database with name ${dbName}: ${event}`);
    };
}
export function putPair(key, value, secret) {
    return __awaiter(this, void 0, void 0, function* () {
        const store = database.transaction("pairs", "readwrite").objectStore("pairs");
        const request = store.put({ key, value });
        request.onsuccess = () => __awaiter(this, void 0, void 0, function* () {
            yield dotnetReference.invokeMethodAsync("PairSaved");
        });
    });
}
export function deletePair(id) {
    return __awaiter(this, void 0, void 0, function* () {
        const store = database.transaction("pairs", "readwrite").objectStore("pairs");
        const request = store.delete(id);
        request.onsuccess = () => __awaiter(this, void 0, void 0, function* () {
            yield dotnetReference.invokeMethodAsync("PairDeleted", id);
        });
    });
}
export function getAllPairs() {
    const store = database.transaction("pairs", "readonly").objectStore("pairs");
    const request = store.getAll();
    request.onsuccess = () => __awaiter(this, void 0, void 0, function* () {
        yield dotnetReference.invokeMethodAsync("UpdatePairs", request.result);
    });
}
//# sourceMappingURL=indexeddbWrapper.js.map