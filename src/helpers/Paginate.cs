namespace Store.Helpers;

public static class Paginate {
    public static List<T> Function<T>(List<T> itemsList, int page = 1, int limit = 25) {
        
        if(limit > 25) limit = 25;
        if(itemsList.Count < ((page - 1) * limit)) page = 1;
        if(itemsList.Count < (page  * limit)) {
            limit = itemsList.Count;
        } else {
            limit = page * limit;
        };
        return itemsList[((page - 1) * limit)..limit];
    }
}