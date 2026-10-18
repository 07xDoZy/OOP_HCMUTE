package dao;

import java.util.List;
import java.util.Optional;

public interface IGenericDAO<T> {
    void add(T t);
    boolean delete(String ma);
    Optional<T> findById(String ma);
    List<T> findAll();
}