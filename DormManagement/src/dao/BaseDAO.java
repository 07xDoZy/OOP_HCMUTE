package dao;

import java.util.Collections;
import java.util.List;
import java.util.Optional;
import java.util.function.Function;

public abstract class BaseDAO<T> implements IGenericDAO<T> {
    private final List<T> data;
    private final Function<T, String> idGetter;

    protected BaseDAO(List<T> data, Function<T, String> idGetter) {
        this.data = data;
        this.idGetter = idGetter;
    }

    @Override
    public void add(T t) {
        data.add(t);
    }

    @Override
    public boolean delete(String ma) {
        return data.removeIf(t -> idGetter.apply(t).equals(ma));
    }

    @Override
    public Optional<T> findById(String ma) {
        return data.stream().filter(t -> idGetter.apply(t).equals(ma)).findFirst();
    }

    @Override
    public List<T> findAll() {
        return Collections.unmodifiableList(data);   // muốn thêm/xóa phải qua add()/delete()
    }
}