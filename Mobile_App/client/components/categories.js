import React, { useEffect, useState } from 'react';
import { Image, ScrollView, Text, TouchableOpacity, View } from 'react-native';

export default function Categories() {
  const [activeCategory, setActiveCategory] = useState(null);
  const [categories, setCategories] = useState([]);

  useEffect(() => {
    const getListCategories = async () => {
      try {
        const response = await fetch(
          'https://c341-14-191-243-74.ngrok-free.app/api/Category',
          {
            method: 'GET',
          }
        );
        const data = await response.json();
        if (data && data.items && Array.isArray(data.items)) {
          setCategories(data.items);
          // console.log('Data:', data.items);
        } else {
          console.log('Invalid data format:', data);
        }
      } catch (error) {
        console.log('Error fetching categories:', error);
      }
    };

    getListCategories();
  }, []); // Empty dependency array means this effect will run once after initial render
  const handleCategoryPress = (categoryId) => {
    setActiveCategory(categoryId);
  };
  return (
   <View className='mt-4'>
      <ScrollView 
      horizontal
      showsHorizontalScrollIndicator={false}
      className='overflow-visible'
      contentContainerStyle={{
        paddingHorizontal: 15
      }}>
        {
            categories.map((category, index)=>{
                let isActive = category._id == activeCategory
                let btnClass = isActive? 'bg-gray-600' : 'bg-gray-200'
                let textClass = isActive? 'font-semibold text-orange-500' : 'text-gray-800'
                return (
                    <View key={index} 
                    className='flex justify-center items-center mr-6'>
                        <TouchableOpacity 
                        onPress={() => handleCategoryPress(category._id)}
                        className={'rounded-full shadow bg-gray-200 ' + btnClass}>
                        <Image
                          style={{ width: 80, height: 80, borderRadius: 50, backgroundColor: '#fff' }}
                          source={{ uri: category.pathImage }}
                        />
                        </TouchableOpacity>
                            <Text className={'text-base '+ textClass} >{category.name}</Text>
                    </View>
                )
            })
        }
      </ScrollView>
    </View>
  )
}
