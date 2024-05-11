namespace Store.Helpers;

public static class Paginate {
    public static List<T> Function<T>(List<T> itemsList, int page = 1, int limit = 50) {
        
        if(limit > 50) limit = 50;
        if(itemsList.Count < ((page - 1) * limit)) page = 1;
        if(itemsList.Count < (page  * limit)) {
            limit = itemsList.Count;
        } else {
            limit = page * limit;
        };

        return itemsList[
            ((page - 1) * limit)
            ..
            (page * limit)
        ];
    }
}