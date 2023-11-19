import { useNavigation } from '@react-navigation/native';
import React, { useEffect, useState } from 'react';
import { Image, ScrollView, Text, TouchableOpacity, View } from 'react-native';

export default function Categories() {
  const [activeCategory, setActiveCategory] = useState(null);
  const [categories, setCategories] = useState([]);
  const navigation = useNavigation();

  useEffect(() => {
    const getListCategories = async () => {
      try {
        const response = await fetch(
          'https://7f72-14-191-242-235.ngrok-free.app/api/Category',
          {
            method: 'GET',
          }
        );
        const data = await response.json();
        if (data && data.items && Array.isArray(data.items)) {
          setCategories(data.items);
        } else {
          console.log('Invalid data format:', data);
        }
      } catch (error) {
        console.log('Error fetching categories:', error);
      }
    };

    getListCategories();
  }, []);

  const handleCategoryPress = (categoryId) => {
    setActiveCategory(categoryId);
    navigation.navigate('ProductList', { categoryId }); // Chuyển ID danh mục sang màn hình ProductList
  };

  return (
    <View style={{ marginTop: 4 }}>
      <ScrollView
        horizontal
        showsHorizontalScrollIndicator={false}
        contentContainerStyle={{
          paddingHorizontal: 15,
        }}
      >
        {categories.map((category, index) => {
          let isActive = category.id == activeCategory;
          let btnClass = isActive ? 'bg-gray-600' : 'bg-gray-200';
          let textClass = isActive ? 'font-semibold text-orange-500' : 'text-gray-800';
          return (
            <View key={index} style={{ flex: 1, justifyContent: 'center', alignItems: 'center', marginRight: 6 }}>
              <TouchableOpacity
                onPress={() => handleCategoryPress(category.id)}
                style={{ borderRadius: 50, overflow: 'hidden' }}
              >
                <Image
                  style={{ width: 80, height: 80, borderRadius: 50, backgroundColor: '#fff' }}
                  source={{ uri: category.pathImage }}
                />
              </TouchableOpacity>
              <Text style={{ marginTop: 8 }} className={'text-base ' + textClass}>
                {category.name}
              </Text>
            </View>
          );
        })}
      </ScrollView>
    </View>
  );
}
